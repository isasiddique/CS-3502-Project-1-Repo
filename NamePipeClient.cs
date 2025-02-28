using System;
using System.IO;
using System.IO.Pipes;

public class NamePipeClient
{
    /// The main method to start the client
    public static void Main()
    {
        try
        {
            // Connect to the server
            using (NamedPipeClientStream pipeClientStream = new NamedPipeClientStream(".", "BookingPipe", PipeDirection.InOut))
            {
                Console.WriteLine("Connecting...");
                pipeClientStream.Connect();
                
                // Send a message to the server and read the response
                using (StreamWriter writer = new StreamWriter(pipeClientStream) { AutoFlush = true })
                using (StreamReader reader = new StreamReader(pipeClientStream))
                {
                    writer.WriteLine("Book");
                    string output = reader.ReadLine();
                    Console.WriteLine("Response of the Server: " + output);

                    writer.Close();
                    reader.Close();
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
}