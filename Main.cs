using System;
using System.Threading;
using System.Collections.Generic;
 
// A simple ticket booking system
public class TicketBookingSystem
{
    private int remainingSeats = 10;
    private readonly object bookingLock = new object();
 
    // Method to attempt booking a seat
    public void AttemptBooking(int customerId)
    {
        lock (bookingLock)
        {
            if (remainingSeats > 0)
            {
                int bookedSeatNumber = 11 - remainingSeats; // Calculate actual seat number
                Console.WriteLine($"Customer {customerId} booked seat {bookedSeatNumber}. Remaining: {--remainingSeats}");
            }
            else
            {
                Console.WriteLine($"Customer {customerId} failed to book. No seats available.");
            }
        }
    }
 
    public static void Main(string[] args)
    {
        //  Create a ticket booking system instance and simulate multiple customers
        TicketBookingSystem system = new TicketBookingSystem();
        List<Thread> customers = new List<Thread>();
 
        // Create 12 booking attempts to demonstrate failure cases
        for (int i = 1; i <= 12; i++)
        {
            int customerId = i;
            Thread customerThread = new Thread(() => system.AttemptBooking(customerId));
            customers.Add(customerThread);
        }
 
        // Start and join all customer threads
        customers.ForEach(c => c.Start());
        customers.ForEach(c => c.Join());
    }
}