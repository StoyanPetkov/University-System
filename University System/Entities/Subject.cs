using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace University_System.Entities
{
    public class Subject : BaseEntityWithId,IEquatable<Subject>
    {
        public string Name { get; set; }
        public bool Equals(Subject other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return Name.Equals(other.Name) && Id.Equals(other.Id);
        }
        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashSubjectName = Name == null ? 0 : Name.GetHashCode();

            //Get hash code for the Code field.
            int hashSubjectId = Id.GetHashCode();

            //Calculate the hash code for the product.
            return hashSubjectName ^ hashSubjectId;
        }
    }
}