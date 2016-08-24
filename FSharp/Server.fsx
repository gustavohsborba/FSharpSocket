open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading
open System.Text


// Functions to treat bad path strings
let makeFilePath  (str:string) =
  let cleanPath = String.Join("", str.Split(Path.GetInvalidPathChars()));
  cleanPath

let makeFileName (str:string) =
  let cleanFileName = String.Join("", str.Split(Path.GetInvalidFileNameChars()));
  cleanFileName

let makeName (str:string) = 
  makeFilePath(makeFileName(str))


type Socket with
  member socket.AsyncAccept() = Async.FromBeginEnd(socket.BeginAccept, socket.EndAccept)

type Server() =
  static member Start(?path:string, ?port) = 
    let defPath = "out"
    let path = defaultArg path defPath
    let port = defaultArg port 8593
    let ipAddress = IPAddress.Any
    let endpoint = IPEndPoint(ipAddress, port)
    let cts = new CancellationTokenSource()
    let listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    listener.Bind(endpoint)
    listener.Listen(int SocketOptionName.MaxConnections)
    printfn "Running server on localhost..."
    printfn "Files targered to be saved at %s" path
    printfn "Started listening on port %d" port
    
    let rec loop() = async {
      printfn "Waiting for request ..."
      let! socket = listener.AsyncAccept()
      let inputStream = new NetworkStream(socket, false)
      printfn "Received request"
      
      let bytes: byte[] = Array.create 1024 (byte(0))
      
      inputStream.Read(bytes, 0, 1024) |> ignore
      inputStream.Write(Encoding.UTF8.GetBytes("SUCCESS!"), 0, 8)
      let filename = makeName(System.Text.Encoding.UTF8.GetString(bytes))
      let mutable filepath = makeFilePath(path + "/" + filename)

      printfn "Receiving file: %s" filename
      printfn "file will be saved as %s" filepath  
      
      try
        try
          
          let filep = File.Exists(filepath)
          if filep then
             filepath <- makeFilePath(path + "/" + Path.GetFileNameWithoutExtension(filename) + "(1)" + Path.GetExtension(filename))
          let outputStream = File.Create(filepath, 1)
         
          while inputStream.Read(bytes,0,1) > 0 do
            bytes = Array.create 1 (byte(0)) |> ignore
            outputStream.Write(bytes, 0, 1)
          outputStream.Close()
          
          printfn "Transmission completed Successfuly!"
          printfn "Writing file..."
          printfn "Done!"
        
        with e -> printfn "An error occurred: %s" e.Message
      finally
        socket.Close()
      return! loop() 
    }

    Async.Start(loop(), cancellationToken = cts.Token)
    { 
      new IDisposable with member x.Dispose() = cts.Cancel(); listener.Close() 
    }






// Running from FSI, the script name is first, and other args after
match fsi.CommandLineArgs with
    | [| scriptName; folder; serverport |] ->
        let disposable = Server.Start(folder, System.Int32.Parse(serverport))
        Thread.Sleep(3600 * 1000) // wait an hour to dispose CPU resources.
        disposable.Dispose()
    | [| scriptName |] ->
        let disposable = Server.Start()
        Thread.Sleep(3600 * 1000) // wait an hour to dispose CPU resources.
        disposable.Dispose()
    | _ ->
        printfn "USAGE: fsharpi Server.fsx (save_path) (port)"