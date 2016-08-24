package ServerSide;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;


public class Server {

    public static String makeString(char [] vector){
        String ret = "";
        for(int i=0; i<vector.length && vector[i]!='\0'; i++){
            ret += vector[i];
        }
        return ret;
    }


    public static void main(String [] args) throws IOException {

        ServerSocket server = new ServerSocket(8593);
        Socket socket = server.accept();
        BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()));

        char buffer[] = new char[1024];
        int bytes;
        bytes = in.read(buffer);

        File dir = new File("/home/gustavo/workspace/Redes/Socket/out");
        System.out.print(buffer);
        File file = new File(dir, makeString(buffer));
        file.createNewFile();

        FileWriter writer = new FileWriter(file);

        while (bytes > -1) {
            writer.flush();
            bytes = in.read(buffer);
            writer.write(buffer);
        }

        socket.close();
        server.close();
        writer.flush();
        writer.close();
    }
}
