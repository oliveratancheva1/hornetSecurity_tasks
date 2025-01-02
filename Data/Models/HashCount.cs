using System;
using System.ComponentModel.DataAnnotations;

namespace HashProcessorApp.Data.Models
{
    public class HashCount
    {
        // Primary key
        public int Id { get; set; }

        // Hash value, marked as required and non-nullable
        [Required(ErrorMessage = "Hash is required.")]
        public string Hash { get; set; } = string.Empty;

        // Date value, non-nullable
        public DateTime Date { get; set; }

        // Constructor to initialize properties
        public HashCount(string hash, DateTime date)
        {
            // Ensure the Hash is not null or empty when setting it
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentException("Hash cannot be null or empty", nameof(hash));

            Hash = hash;
            Date = date;
        }

        // Parameterless constructor for EF Core (required for EF Core to work properly)
        public HashCount() { }
    }
}
