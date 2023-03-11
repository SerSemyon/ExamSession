using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;
namespace ExamSession
{
    class Program
    {
        static int CreateTables(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(@"
CREATE TABLE Study_Group
(
	number_group CHARACTER VARYING(10),
    Specialization CHARACTER VARYING(30),
    elder INTEGER,
    Tutor_ID INTEGER,
     Number_of_students INTEGER,
	PRIMARY KEY(number_group)
);

CREATE TABLE Student
(
    Id INT,
    Name CHARACTER VARYING(15),
	Last_Name CHARACTER VARYING(15),
    Age INTEGER CHECK(Age >0),
	Phone_Number CHARACTER VARYING(10) CHECK (Phone_Number LIKE '__________'),
	Email CHARACTER VARYING(20) CHECK(Email LIKE '%@%.%'),
	Number_of_Group CHARACTER VARYING(10)
);

CREATE TABLE Item
(
	Id INT,
    Name CHARACTER VARYING(25), 
	number_of_hours INTEGER,
	Description CHARACTER VARYING(500)
);


CREATE TABLE Teacher
(
	Id INT,
    Name CHARACTER VARYING(15),
	Last_Name CHARACTER VARYING(15),
	Patronymic CHARACTER VARYING(15),
    Age INTEGER CHECK(Age >18),
	Phone_Number CHARACTER VARYING(10) CHECK (Phone_Number LIKE '__________'),
	Email CHARACTER VARYING(40) CHECK(Email LIKE '%@%.%')
);

CREATE TABLE Assessment
(
	Id INT,
	Item_Code INTEGER,
	Student_id INTEGER,
	Score INTEGER CHECK (Score>=0 AND score <=100),
 date_exam DATE
); 

CREATE TABLE department
(
	Id INT,
    Name CHARACTER VARYING(35),
    audience_number INTEGER CHECK(audience_number >0),
	Phone_Number CHARACTER VARYING(10) CHECK (Phone_Number LIKE '__________'),
	Email CHARACTER VARYING(20) CHECK(Email LIKE '%@%.%'),
	number_of_employees INTEGER CHECK(number_of_employees > 0),
	head_of_the_department INTEGER
);

CREATE TABLE teacher_item (
	Id INT,
	Item_Code INTEGER,
	teacher_id INTEGER
);
                ", connection);
            return command.ExecuteNonQuery();
        }

        static int CreateDataInTables(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(@"
INSERT INTO study_group 
(number_group, Specialization, number_of_students)
VALUES
('09-021','Прикладная математика', 18),
('09-022','Прикладная математика', 20);

INSERT INTO student
(name, last_name, age, phone_number, email, Number_of_Group)
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
SET elder = 1, tutor_id=1
WHERE number_group = '09-021';
UPDATE study_group
SET elder = 2, tutor_id=3
WHERE number_group = '09-022';

INSERT INTO assessment(item_code, student_id, score, date_exam)
VALUES (1, 1, 56, '2021-01-13'),(2, 1, 86, '2021-01-13'),(3, 1, 76, '2021-01-13'),
(1, 1, 47, '2021-01-13'),(2, 1, 86, '2021-01-13'),(3, 1, 76, '2021-01-13'),
(1, 2, 96, '2021-06-18'),(2, 2, 84, '2021-06-18'),(3, 2, 16, '2021-06-18'),
(1, 3, 56, '2021-06-18'),(2, 3, 48, '2021-06-18'),(3, 3, 72, '2021-06-18'),
(1, 3, 27, '2022-01-18'),(2, 3, 76, '2022-01-18'),(3, 3, 79, '2022-01-18');
                ", connection);
            return command.ExecuteNonQuery();
        }
        static void RebootDataBase(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("DROP DATABASE ExamSession;", connection);
            command.ExecuteNonQuery();
            command.CommandText = "CREATE DATABASE ExamSession;";
            command.ExecuteNonQuery();
            command.CommandText = "USE ExamSession";
            command.ExecuteNonQuery();

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
        static async Task Main(string[] args)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                Console.WriteLine("Подключение открыто");
                // Вывод информации о подключении
                Console.WriteLine("Свойства подключения:");
                Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
                Console.WriteLine($"\tБаза данных: {connection.Database}");
                Console.WriteLine($"\tСервер: {connection.DataSource}");
                Console.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
                Console.WriteLine($"\tСостояние: {connection.State}");
                Console.WriteLine($"\tWorkstationld: {connection.WorkstationId}");
                RebootDataBase( connection );
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // если подключение открыто
                if (connection.State == ConnectionState.Open)
                {
                    // закрываем подключение
                    await connection.CloseAsync();
                    Console.WriteLine("Подключение закрыто...");
                }
            }
            connectionString = "Server=(localdb)\\mssqllocaldb;Database=ExamSession;Trusted_Connection=True;";
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine(CreateTables(connection));
                Console.WriteLine(CreateDataInTables(connection));


                string sqlExpression = "SELECT * FROM assessment";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                await ShowTable(reader);
                
                }
            Console.WriteLine("Программа завершила работу.");
            Console.Read();
        }
    }
}