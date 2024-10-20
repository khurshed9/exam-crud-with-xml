namespace examWithXML.Entities;

    public class Supplier
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ContactPerson { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;
    }