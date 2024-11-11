package server;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;

public class Server {
    public static void main(String[] args) throws IOException {
        ServerSocket server = new ServerSocket(1111);
        while(true){
            Socket clientSocket = server.accept();
            Thread thread = new Thread(new Session(clientSocket));
            thread.start();
        }
    }
}
