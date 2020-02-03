using System;

namespace Present.Entity.Employee
{
    public class EmployeeRaw
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        protected bool Equals(EmployeeRaw other)
        {
            return Name == other.Name && Surname == other.Surname;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeRaw) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Surname);
        }
    }
}