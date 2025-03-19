namespace backendSpark.Model.Entities;

public class Organizer { 
   public Organizer(string orgId){OrgId = orgId;} 
   public Organizer(){}
   public Organizer(string orgId, string name, string email, string password)
   {
      OrgId = orgId;
      Name = name;
      Email = email;
      Password = password;
   }

   public string OrgId { get; set; } 
   public string Name { get; set; } 
   public string Email { get; set; }
   public string Password { get; set; }   

}


// EXAMPLE CODE PROVIDED IN CLASS
/*

public class Student { 
   public Student(int id){Id = id;} 
   public int Id { get; set; } 
   public string FirstName { get; set; } 
   public string LastName { get; set; } 
   public int StudyProgramId { get; set; } 
   public DateTime DateOfBirth { get; set; } 
   public string Email { get; set; } 
   public string Phone { get; set; } 
}

*/