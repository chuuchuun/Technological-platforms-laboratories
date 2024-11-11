package server;

import org.example.Message;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.Socket;

public class Session implements Runnable {
    private Socket clientSocket;
    public Session(Socket clientSocket){
        this.clientSocket = clientSocket;
    }
    static int count = 0;
    @Override
    public void run() {
        try {
            ObjectOutputStream out = new ObjectOutputStream(clientSocket.getOutputStream());
            ObjectInputStream in = new ObjectInputStream(clientSocket.getInputStream());

            String id = String.valueOf(count);
            count++;
            out.writeObject(id);
            out.flush();

            out.writeObject("Ready");
            out.flush();

            int messageNum = in.readInt();
            out.writeObject("Ready for messages!");
            out.flush();

            for(int i =0; i < messageNum; i++){
                Message message = (Message) in.readObject();
                System.out.println(message);
            }
            out.writeObject("Finished");
            out.flush();
            out.close();
            in.close();
            clientSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
           e.printStackTrace();
        }
    }
}
