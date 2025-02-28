using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

public class NamePipeServer
{
    private static int seats = 10;
    private static object lock_Key = new object();

    /// The main method to start the server
    public static void Main()
    {
        Console.WriteLine("The ticket server is running.");
        Thread sthread = new Thread(StartServer);
        sthread.Start();
    }

    /// The method to start the server
    private static void StartServer()
    {
        while (true)
        {
            try
            {
                using (NamedPipeServerStream namedPipeServer = new NamedPipeServerStream("BookingPipe", PipeDirection.InOut, 10))
                {
                    Console.WriteLine("Waiting for connection to be established");
                    namedPipeServer.WaitForConnection();

                    using (StreamReader reader = new StreamReader(namedPipeServer))
                    using (StreamWriter writer = new StreamWriter(namedPipeServer) { AutoFlush = true })
                    {
                        string input = reader.ReadLine();
                        if (input == "Book")
                        {
                            lock (lock_Key)
                            {
                                Console.WriteLine("Booking Ticket");
                                Console.WriteLine("Seats Available: " + seats);
                                if (seats > 0)
                                {
                                    seats--;
                                    writer.WriteLine("Your Ticket has been booked! Seats Remaining: " + seats);
                                }
                                else
                                {
                                    writer.WriteLine("Sorry, there are no more seats available.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}