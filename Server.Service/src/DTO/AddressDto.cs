using Server.Core.src.Entity;

namespace Server.Service.src.DTO
{
    public class AddressReadDto
    {
        public string AddressLine { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Landmark { get; set; }
        public Guid UserId { get; set; }


        public AddressReadDto Transform(Address address)
        {
            FirstName = address.FirstName;
            LastName = address.LastName;
            Street = address.Street;
            City = address.City;
            Country = address.Country;
            Postcode = address.Postcode;
            PhoneNumber = address.PhoneNumber;
            Landmark = address.Landmark;
            AddressLine = address.AddressLine;
            UserId = address.UserId;
            return this;
        }
    }

    public class AddressCreateDto
    {
        public string AddressLine { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Landmark { get; set; }

        public AddressCreateDto(string addressLine, string street, string city, string country, string postcode, string phoneNumber, string firstName, string lastName, string landmark)
        {
            AddressLine = addressLine;
            Street = street;
            City = city;
            Country = country;
            Postcode = postcode;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Landmark = landmark;
        }
        public Address CreateAddress(Guid userId)
        {
            return new Address(AddressLine, Street, City, Country, Postcode, PhoneNumber, FirstName, LastName, Landmark, userId);
        }
    }

    public class AddressUpdateDto
    {
        public string AddressLine { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Landmark { get; set; }

        public AddressUpdateDto(string addressLine, string street, string city, string country, string postcode, string phoneNumber, string firstName, string lastName, string landmark)
        {
            AddressLine = addressLine;
            Street = street;
            City = city;
            Country = country;
            Postcode = postcode;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Landmark = landmark;
        }

        public Address UpdateAddress(Address oldAddress)
        {
            oldAddress.FirstName = FirstName;
            oldAddress.LastName = LastName;
            oldAddress.Street = Street;
            oldAddress.AddressLine = AddressLine;
            oldAddress.City = City;
            oldAddress.Country = Country;
            oldAddress.Postcode = Postcode;
            oldAddress.PhoneNumber = PhoneNumber;
            oldAddress.Landmark = Landmark;
            return oldAddress;
        }
    }
}