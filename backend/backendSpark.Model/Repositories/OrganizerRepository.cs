using System; 
using backendSpark.Model.Entities; 
using Microsoft.Extensions.Configuration; 
using Npgsql; 
using NpgsqlTypes; 
namespace backendSpark.Model.Repositories; 
public class OrganizerRepository : BaseRepository 
{ 
   public OrganizerRepository(IConfiguration configuration) : base(configuration) 
   { 
   } 
   public Organizer GetOrganizerById(string orgId) 
   { 
      NpgsqlConnection dbConn = null; 
      try 
      { 
         //create a new connection for database 
         dbConn = new NpgsqlConnection(ConnectionString); 
         //creating an SQL command 
         var cmd = dbConn.CreateCommand(); 
         cmd.CommandText = "select * from organizer where org_id = @org_id"; 
         cmd.Parameters.Add("@org_id", NpgsqlDbType.Varchar).Value = orgId; 
         //call the base method to get data 
         var data = GetData(dbConn, cmd); 
         if (data != null) 
         { 
            if (data.Read()) //every time loop runs it reads next like from fetched rows 
            { 
               return new Organizer(data["org_id"].ToString()) 
               { 
                  Name = data["name"].ToString(), 
                  Email = data["email"].ToString(), 
                  Password = data["hash_password"].ToString() 
               }; 
            } 
         } 
         return null; 
      } 
      finally 
      { 
         dbConn?.Close(); 
      } 
   } 
   public List<Organizer> GetOrganizers() 
   { 
      NpgsqlConnection dbConn = null; 
      var organizers = new List<Organizer>(); 
      try 
      { 
         //create a new connection for database 
         dbConn = new NpgsqlConnection(ConnectionString); 
         //creating an SQL command 
         var cmd = dbConn.CreateCommand(); 
         cmd.CommandText = "select * from organizer"; 
         //call the base method to get data 
         var data = GetData(dbConn, cmd); 
         if (data != null) 
         { 
            while (data.Read()) //every time loop runs it reads next like from fetched rows 
            { 
               Organizer o = new Organizer(data["org_id"].ToString()) 
               { 
                  Name = data["name"].ToString(), 
                  Email = data["email"].ToString(), 
                  Password = data["hash_password"].ToString() 
               }; 
               organizers.Add(o); 
            } 
         } 
         return organizers; 
      } 
      finally 
      { 
         dbConn?.Close(); 
      } 
   } 
   //add a new organizer 
   public bool InsertOrganizer(Organizer o) 
   { 
      NpgsqlConnection dbConn = null; 
      try 
      { 
         dbConn = new NpgsqlConnection(ConnectionString); 
         var cmd = dbConn.CreateCommand(); 
         cmd.CommandText = @" 
insert into organizer 
(org_id, name, email, hash_password) 
values 
(@org_id, @name, @email, @hash_password) 
"; 
         //adding parameters in a better way 
         cmd.Parameters.AddWithValue("@org_id", NpgsqlDbType.Varchar, o.OrgId);
         cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, o.Name); 
         cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, o.Email); 
         cmd.Parameters.AddWithValue("@hash_password", NpgsqlDbType.Text, o.Password);

         //will return true if all goes well 
         bool result = InsertData(dbConn, cmd); 
         return result; 
      } 
      finally 
      { 
         dbConn?.Close(); 
      } 
   } 
   public bool UpdateOrganizer(Organizer o) 
   { 
      var dbConn = new NpgsqlConnection(ConnectionString); 
      var cmd = dbConn.CreateCommand(); 
      cmd.CommandText = @" 
update organizer set 
    name=@name, 
    email=@email, 
    hash_password=@hash_password
where 
org_id = @org_id"; 
      cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, o.Name); 
      cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, o.Email); 
      cmd.Parameters.AddWithValue("@hash_password", NpgsqlDbType.Text, o.Password); 
      cmd.Parameters.AddWithValue("@org_id", NpgsqlDbType.Varchar, o.OrgId); 
      bool result = UpdateData(dbConn, cmd); 
      return result; 
   } 
   public bool DeleteOrganizer(string orgId) 
   { 
      var dbConn = new NpgsqlConnection(ConnectionString); 
      var cmd = dbConn.CreateCommand(); 
      cmd.CommandText = @" 
delete from organizer 
where org_id = @org_id 
"; 
      //adding parameters in a better way 
      cmd.Parameters.AddWithValue("@org_id", NpgsqlDbType.Varchar, orgId); 
      //will return true if all goes well 
      bool result = DeleteData(dbConn, cmd); 
      return result; 
   } 
}


// EXAMPLE CODE PROVIDED IN CLASS

/* 

using System; 
using CourseAdminSystem.Model.Entities; 
using Microsoft.Extensions.Configuration; 
using Npgsql; 
using NpgsqlTypes; 
namespace CourseAdminSystem.Model.Repositories; 
public class StudentRepository : BaseRepository 
{ 
   public StudentRepository(IConfiguration configuration) : base(configuration) 
   { 
   } 
   public Student GetStudentById(int id) 
   { 
      NpgsqlConnection dbConn = null; 
      try 
      { 
         //create a new connection for database 
         dbConn = new NpgsqlConnection(ConnectionString); 
         //creating an SQL command 
         var cmd = dbConn.CreateCommand(); 
         cmd.CommandText = "select * from student where id = @id"; 
         cmd.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id; 
         //call the base method to get data 
         var data = GetData(dbConn, cmd); 
         if (data != null) 
         { 
            if (data.Read()) //every time loop runs it reads next like from fetched rows 
            { 
               return new Student(Convert.ToInt32(data["id"])) 
               { 
                  FirstName = data["firstname"].ToString(), 
                  LastName = data["lastname"].ToString(), 
                  StudyProgramId = (int)data["studyprogramid"], 
                  DateOfBirth = Convert.ToDateTime(data["dob"]), 
                  Email = data["email"].ToString(), 
                  Phone = data["phone"].ToString() 
               }; 
            } 
         } 
         return null; 
      } 
      finally 
      { 
         dbConn?.Close(); 
      } 
   } 
   public List<Student> GetStudents() 
   { 
      NpgsqlConnection dbConn = null; 
      var students = new List<Student>(); 
      try 
      { 
         //create a new connection for database 
         dbConn = new NpgsqlConnection(ConnectionString); 
         //creating an SQL command 
         var cmd = dbConn.CreateCommand(); 
         cmd.CommandText = "select * from student"; 
         //call the base method to get data 
         var data = GetData(dbConn, cmd); 
         if (data != null) 
         { 
            while (data.Read()) //every time loop runs it reads next like from fetched rows 
            { 
               Student s = new Student(Convert.ToInt32(data["id"])) 
               { 
                  FirstName = data["firstname"].ToString(), 
                  LastName = data["lastname"].ToString(), 
                  StudyProgramId = (int)data["studyprogramid"], 
                  DateOfBirth = Convert.ToDateTime(data["dob"]), 
                  Email = data["email"].ToString(), 
                  Phone = data["phone"].ToString() 
               }; 
               students.Add(s); 
            } 
         } 
         return students; 
      } 
      finally 
      { 
         dbConn?.Close(); 
      } 
   } 
   //add a new student 
   public bool InsertStudent(Student s) 
   { 
      NpgsqlConnection dbConn = null; 
      try 
      { 
         dbConn = new NpgsqlConnection(ConnectionString); 
         var cmd = dbConn.CreateCommand(); 
         cmd.CommandText = @" 
insert into student 
(firstname,lastname, studyprogramid, dob, email, phone) 
values 
(@firstname,@lastname, @studyprogramid, @dob, @email, @phone) 
"; 
         //adding parameters in a better way 
         cmd.Parameters.AddWithValue("@firstname", NpgsqlDbType.Text, s.FirstName); 
         cmd.Parameters.AddWithValue("@lastname", NpgsqlDbType.Text, s.LastName); 
         cmd.Parameters.AddWithValue("@studyprogramid", NpgsqlDbType.Integer, 
s.StudyProgramId); 
         cmd.Parameters.AddWithValue("@dob", NpgsqlDbType.Date, s.DateOfBirth); 
         cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, s.Email); 
         cmd.Parameters.AddWithValue("@phone", NpgsqlDbType.Text, s.Phone); 
         //will return true if all goes well 
         bool result = InsertData(dbConn, cmd); 
         return result; 
      } 
      finally 
      { 
         dbConn?.Close(); 
      } 
   } 
   public bool UpdateStudent(Student s) 
   { 
      var dbConn = new NpgsqlConnection(ConnectionString); 
      var cmd = dbConn.CreateCommand(); 
      cmd.CommandText = @" 
update student set 
    firstname=@firstname, 
    lastname=@lastname, 
    studyprogramid=@studyprogramid, 
    dob=@dob, 
    email=@email, 
    phone=@phone 
where 
id = @id"; 
      cmd.Parameters.AddWithValue("@firstname", NpgsqlDbType.Text, s.FirstName); 
      cmd.Parameters.AddWithValue("@lastname", NpgsqlDbType.Text, s.LastName); 
      cmd.Parameters.AddWithValue("@studyprogramid", NpgsqlDbType.Integer, 
s.StudyProgramId); 
      cmd.Parameters.AddWithValue("@dob", NpgsqlDbType.Date, s.DateOfBirth); 
      cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, s.Email); 
      cmd.Parameters.AddWithValue("@phone", NpgsqlDbType.Text, s.Phone); 
      cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, s.Id); 
      bool result = UpdateData(dbConn, cmd); 
      return result; 
   } 
   public bool DeleteStudent(int id) 
   { 
      var dbConn = new NpgsqlConnection(ConnectionString); 
      var cmd = dbConn.CreateCommand(); 
      cmd.CommandText = @" 
delete from student 
where id = @id 
"; 
      //adding parameters in a better way 
      cmd.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id); 
      //will return true if all goes well 
      bool result = DeleteData(dbConn, cmd); 
      return result; 
   } 
}

*/