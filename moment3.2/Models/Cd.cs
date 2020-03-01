using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace moment3._2.Models
{
    public class Cd
    {
        public int CdId { get; set;}
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }

        public bool Avalable { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
        [NotMapped]
        public Track Track { get; set; }
    
        public ICollection<Track> Tracks { get; set; }
        public ICollection<Rent> Rented { get; set; }


    }
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
public ICollection<Cd> Cds { get; set; }
    }
    public class Rent
    {
        public int RentId { get; set; }
        public string Name { get; set; }
        public DateTime RentDate { get; set; }
        public int cdId { get; set; }
        public Cd Cd { get; set; }
    }
    public class Track
    {
        public int TrackId { get; set; }
        public string Name { get; set; }
        public int cdId { get; set; }
        public Cd Cd { get; set; }
 

    }
    public class TrackList
    {
        public List<Track> Tracks { get; set; }
    }
}
