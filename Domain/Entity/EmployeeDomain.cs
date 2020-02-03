using System;

namespace Domain.Entity
{
    public class EmployeeDomain
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        protected bool Equals(EmployeeDomain other)
        {
            return Id.Equals(other.Id) && Name == other.Name && Surname == other.Surname;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeDomain) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname);
        }
    }
}