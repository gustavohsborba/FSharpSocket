open System
open System.IO
open System.Net
open System.Net.Sockets
open System.Threading
open System.Text

type Socket with
  member socket.AsyncAccept() = Async.FromBeginEnd(socket.BeginAccept, socket.EndAccept)

// Running from FSI, the script name is first, and other args after
match fsi.CommandLineArgs with
    | [| scriptName; filepath; hostname |] ->
        try
            let port = 8593
            let ipAddress = Dns.GetHostEntry(hostname).AddressList.[0]
            let endpoint = IPEndPoint(ipAddress, port)
            let cts = new CancellationTokenSource()
            let socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            printfn "Trying to connect to server..."
            socket.Connect(endpoint)
            
            let bytes = Encoding.UTF8.GetBytes(Path.GetFileName(filepath))
            //socket.Send(bytes) |> ignore

            let stream = new NetworkStream(socket, false)
            stream.Write(bytes, 0, bytes.Length)
            let result = stream.Read(bytes, 0, 7)
            if result >= 7 then
                printfn "Connection Stabilished!"
            stream.Flush()

            let file = new FileStream(filepath, FileMode.Open)
            let mutable reading = file.Length
            while reading > 0L do
                let bite : byte = byte (file.ReadByte())
                let bites : byte[] = ([|bite|])
                stream.Write(bites, 0, 1)
                reading <- reading - 1L
            socket.Shutdown(SocketShutdown.Both)
            stream.Close()
            socket.Close()
            file.Close()
            printfn "File %s Transfered with success!" (Path.GetFileName(filepath))
        with e -> printfn "An error occurred: %s" e.Message
    | _ ->
        printfn "USAGE: fsharpi Client.fsx (save_path) (port)"