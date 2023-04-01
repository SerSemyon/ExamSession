using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Azure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using ExamSession.Tables;

namespace ExamSession
{
    class Program
    {
        //public static string connectionString = "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;";
        public static string connectionString = "Server=localhost;Port=5432;Database=ExamSession;User ID=postgres; Password=503992";
        static void CreateTables(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(@"
CREATE TABLE Study_Group
(
	number_group CHARACTER VARYING(10),
    Specialization CHARACTER VARYING(30),
    elder INTEGER,
    tutor INTEGER,
    number_of_students INTEGER,
	PRIMARY KEY(number_group)
);

CREATE TABLE Student
(
    student_id INT IDENTITY,
    Name NVARCHAR(15) COLLATE Cyrillic_General_CI_AS,
	Last_Name CHARACTER VARYING(15),
    age INTEGER 
		CHECK(age >0),
	phone_number CHARACTER VARYING(16),
	email CHARACTER VARYING(20) 
		CHECK(email LIKE '%@%.%'),
	number_group CHARACTER VARYING(10),
	FOREIGN KEY (number_group) REFERENCES Study_Group (number_group) ON DELETE SET NULL
);

CREATE TABLE Item
(
	item_id INT IDENTITY,
    name CHARACTER VARYING(40), 
	number_of_hours INTEGER,
	description CHARACTER VARYING(500)
);


CREATE TABLE Teacher
(
	teacher_id INT IDENTITY,
    name CHARACTER VARYING(15),
	last_name CHARACTER VARYING(15),
	patronymic CHARACTER VARYING(15),
    age INTEGER 
		CHECK(age >18),
	phone_number CHARACTER VARYING(16),
	email CHARACTER VARYING(40) 
		CHECK(email LIKE '%@%.%')
);

CREATE TABLE Assessment
(
	assessment_id INTEGER IDENTITY,
    item_id INT,
    student_id INT,
	score INTEGER 
		CHECK (score>=0 AND score <=100),
 	exam_date DATE


); 




CREATE TABLE department
(
	department_id  INT IDENTITY,
    name CHARACTER VARYING(35),
    audience_number INTEGER 
		CHECK(audience_number >0),
	phone_number CHARACTER VARYING(16),
	email CHARACTER VARYING(20) 
		CHECK(email LIKE '%@%.%'),
	number_of_employees INTEGER 
		CHECK(number_of_employees > 0),
	head_of_the_department INTEGER UNIQUE

);

CREATE TABLE teacher_item (
	Id INT IDENTITY,
	item_id INTEGER,
	teacher_id INTEGER


);
                ", connection);
            command.ExecuteNonQuery();
        }

        static void CreateDataInTables(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(@"
INSERT INTO Teacher
(name, last_name, age, phone_number, email, patronymic)
VALUES ('Иван','Алексеев', 48, '8123456789', 'IVAN@gmail.com', 'Георгиевич'),
('Михаил','Горячев', 50, '8127456789', 'Misha@gmail.com', 'Юрьевич'),
('Георгий','Костин', 48, '8164456789', 'Geo@yandex.com', 'Артёмович'),
('Олег','Федоров', 89, '8123454789', 'Oleg@gmail.com', 'Семёнович'),
('Татьяна','Панкратова', 48, '8126556789', 'Tan@gmail.com', 'Викторовна'),
('Ольга','Леонтьева', 38, '8125356789', 'Olga@gmail.com', 'Николаевна');

INSERT INTO study_group 
(number_group, Specialization, number_of_students)
VALUES
('09-021','Прикладная математика', 18),
('09-022','Прикладная математика', 20);

INSERT INTO student
(name, last_name, age, phone_number, email, number_group)
VALUES ('Иван','Алексеев', 18, '8123456789', 'IVAN@gmail.com', '09-022'),
('Михаил','Горячев', 20, '8127456789', 'Misha@gmail.com', '09-022'),
('Георгий','Костин', 18, '8164456789', 'Geo@yandex.com', '09-021'),
('Олег','Федоров', 19, '8123454789', 'Oleg@gmail.com', '09-021'),
('Татьяна','Панкратова', 18, '8126556789', 'Tan@gmail.com', '09-021'),
('Ольга','Леонтьева', 18, '8125356789', 'Olga@gmail.com', '09-022');

INSERT INTO Item (name, number_of_hours,Description)
VALUES 
('Методы оптимизации', 130, 'Изучаются методы оптимизации(поиска экстремума функции.)'),
('Философия', 56, 'Поднимаются вопросы смысла жизни.'),
('Мат. моделирование', 180, 'Объясняется, как использовать математический аппарат для описания физических процессов.');

INSERT INTO department
(name, audience_number, Phone_Number,Email,number_of_employees,head_of_the_department)
VALUES
('Кафедра Прикладной математики', 1010, '8234356937', 'maths@stud.ru', 7, 2),
('Кафедра ИИ', 1201, '8234157937', 'department@stud.ru', 12, 5);

UPDATE study_group
SET elder = 1, tutor=1
WHERE number_group = '09-021';
UPDATE study_group
SET elder = 2, tutor=3
WHERE number_group = '09-022';

INSERT INTO assessment(item_id, student_id, score, exam_date)
VALUES (1, 1, 56, '2021-01-13'),(2, 1, 86, '2021-01-13'),(3, 1, 76, '2021-01-13'),
(1, 1, 47, '2021-01-13'),(2, 1, 86, '2021-01-13'),(3, 1, 76, '2021-01-13'),
(1, 2, 96, '2021-06-18'),(2, 2, 84, '2021-06-18'),(3, 2, 16, '2021-06-18'),
(1, 3, 56, '2021-06-18'),(2, 3, 48, '2021-06-18'),(3, 3, 72, '2021-06-18'),
(1, 3, 27, '2022-01-18'),(2, 3, 76, '2022-01-18'),(3, 3, 79, '2022-01-18');
                ", connection);
            command.ExecuteNonQuery();
        }
        static void CreateDataBase(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("DROP DATABASE ExamSession;", connection);
            command.ExecuteNonQuery();
            command.CommandText = "CREATE DATABASE ExamSession;";
            command.ExecuteNonQuery();
            command.CommandText = "USE ExamSession";
            command.ExecuteNonQuery();
            CreateTables(connection);
            CreateDataInTables(connection);
        }
        static async Task ShowTable(SqlDataReader reader)
        {
            if (reader.HasRows) // если есть данные
            {
                // выводим названия столбцов
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write("{0,10}\t", reader.GetName(i));
                }
                Console.WriteLine();
                while (await reader.ReadAsync()) // построчно считываем данные
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write("{0,10}\t", reader.GetValue(i));
                    }
                    Console.WriteLine();
                }
            }
        }
        static void ShowAllTables(SqlConnection connection)
        {
            Console.WriteLine("\n\t\tStudents");
            string sqlExpression = "SELECT * FROM Student";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\n\t\tstudy_group");
            command.CommandText = "SELECT * FROM study_group";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\n\t\tItem");
            command.CommandText = "SELECT * FROM Item";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\n\t\tTeacher");
            command.CommandText = "SELECT * FROM Teacher";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\n\t\tdepartment");
            command.CommandText = "SELECT * FROM department";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\n\t\tassessment");
            command.CommandText = "SELECT * FROM assessment";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();
        }
        static void AddProcedure(SqlConnection connection)
        {
            string proc1 = "CREATE PROCEDURE get_group_list (this_number_group CHARACTER VARYING(10)) LANGUAGE SQL AS $$ SELECT name, last_name FROM student WHERE number_group = this_number_group;$$";
            string proc2 = "CREATE PROCEDURE add_student\r\n    (name CHARACTER VARYING(15),\r\n\tlast_Name CHARACTER VARYING(15),\r\n    age INTEGER,\r\n\tphone_number CHARACTER VARYING(16),\r\n\temail CHARACTER VARYING(20),\r\n\tnumber_group CHARACTER VARYING(10))\r\nLANGUAGE SQL\r\nAS $$\r\n    INSERT INTO student\r\n(name, last_name, age, phone_number, email, number_group)\r\n    VALUES (name, last_name, age, phone_number, email, number_group);\r\n  \r\nUPDATE study_group\r\nSET number_of_students = t.number_of_students\r\nFROM (\r\n\t\tSELECT number_group, count(*) AS number_of_students \r\nFROM student \r\nGROUP BY number_group\r\n) t\r\nWHERE study_group.number_group = t.number_group;\r\nSELECT number_of_students FROM study_group \r\nWHERE study_group.number_group = number_group;\r\n$$\r\n";
            SqlCommand command = new SqlCommand(proc1, connection);
            command.ExecuteNonQuery();
            command.CommandText = proc2;
            command.ExecuteNonQuery();
            Console.WriteLine("Хранимые процедуры добавлены в базу данных.");
        }
        static void ExecuteProcedure(SqlConnection connection)
        {
            string sqlExpression = "get_group_list";

            SqlCommand command = new SqlCommand(sqlExpression, connection);
            // указываем, что команда представляет хранимую процедуру
            command.CommandType = CommandType.StoredProcedure;
            // параметр для ввода имени
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@this_number_group ",
                Value = "09-022"
            };
            // добавляем параметр
            command.Parameters.Add(nameParam);
            // выполняем процедуру
            SqlDataReader reader = command.ExecuteReader();
            ShowTable(reader);
        }
        static void Requests(SqlConnection connection)
        {
            Console.WriteLine("\nВывести данные куратора группы 09-022");
            string sqlExpression = "SELECT * FROM teacher \r\nWHERE teacher_id =\r\n(\r\nSELECT tutor \r\nFROM study_group \r\nWHERE number_group = '09-022'\r\n);\r\n";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\nсписок группы 09-022");
            command.CommandText = "SELECT name, last_name \r\nFROM student \r\nWHERE number_group = '09-022';\r\n";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\nданные студентов, не появившихся ни на одном экзамене");
            command.CommandText = "SELECT * FROM student \r\nWHERE student_id NOT IN \r\n(\r\nSELECT student_id \r\nFROM assessment\r\n); \r\n";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();

            Console.WriteLine("\nсписок студентов, у которых есть хотя бы одна пятёрка");
            command.CommandText = "SELECT * FROM student \r\nWHERE student_id IN \r\n(\r\nSELECT student_id \r\nFROM assessment\r\n WHERE score > 86); \r\n";
            reader = command.ExecuteReader();
            ShowTable(reader);
            reader.Close();
        }
        static async Task Main(string[] args)
        {
            using (ExamSessionContext db = new ExamSessionContext())
            {
                var users = (from student in db.Students
                             where student.NumberGroup == "09-022"
                             select student).ToList();
                foreach (var user in users)
                {
                    Console.WriteLine(user.Name);
                }
                //SqlParameter param = new SqlParameter("@this_number_group", "09-022");
                //var users = db.Students.FromSqlRaw("get_group_list @this_number_group", param).ToList();
                //foreach (var u in users)
                //    Console.WriteLine($"{u.Name}");
            }
            //SqlConnection connection = new SqlConnection(connectionString);
            //try
            //{
            //    connection.Open();
            //    ShowInfoAboutConnection(connection);
            //    CreateDataBase( connection );
            //}
            //catch (SqlException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //    {
            //        await connection.CloseAsync();
            //    }
            //}
            string oldConnectionString = "Server=(localdb)\\mssqllocaldb;Database=ExamSession;Trusted_Connection=True;";
            SqlConnection connection;
            using (connection = new SqlConnection(oldConnectionString))
            {
                connection.Open();
                //Requests(connection);
                ShowAllTables(connection);
                //AddProcedure(connection);
                ExecuteProcedure(connection);
            }
            Console.Read();
        }
    }
}