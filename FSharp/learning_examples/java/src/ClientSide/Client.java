package ClientSide;

import java.io.*;
import java.net.Socket;

public class Client {

    public static void main(String [] args) throws IOException, InterruptedException {

        Socket socket = new Socket("localhost", 8593);
        PrintWriter writer = new PrintWriter(socket.getOutputStream());

        FileReader reader = new FileReader(new File("teste.txt"));
        char buffer[] = new char[1024];
        int bytes;

        writer.write("teste.txt");
        writer.flush();

        BufferedReader in = new BufferedReader(new InputStreamReader(socket.getInputStream()));
        bytes = in.read(buffer);
        System.out.print(buffer);

        do {
            buffer = new char[1024];
            bytes = reader.read(buffer);
            writer.write(buffer);
            writer.flush();
        } while (bytes > -1);

        reader.close();
        writer.close();
        in.close();

    }
}
