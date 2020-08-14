using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Users
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Function that checking if all fields are not empties 
        public int FillCheck()
        {
            string[] input = { id.Text.ToString(), firstname.Text.ToString(), lastname.Text.ToString(),ToString(), email.Text.ToString(), gender.Text.ToString(), birthday.Text.ToString(),phonenumber.Text.ToString() };
            string[] Errors = { "ת.ז", "שם פרטי", "שם משפחה", "תאריך לידה", "אימייל", "מין", "מספר טלפון" };

            for (int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrEmpty(input[i]))
                {
                    MessageBox.Show(" לא מילאת שדה " + Errors[i]);
                    return 0;
                }

            }
            return 1;
        }

        //Function that checking the email insert
        static bool IsValidEmail(string email)
        {
            return Regex.Match(email, @"^[a-zA-Z0-9_]+@[a-zA-Z0-9]+\.[a-zA-Z]+$").Success;
        }


        //Function that checking if the texet insert is legel 
        public int LegelTextInput()
        {
            string[] input = {id.Text.ToString(), phonenumber.Text.ToString()};
            string[] Errors = { "ת.ז", "טלפון"};
            int val;
            for (int i = 0; i < input.Length; i++)
            {

                if (!Int32.TryParse(input[i], out val))
                {
                    MessageBox.Show("   שדה לא חוקי ב" + Errors[i]);
                    return 0;
                }
              else if(IsValidEmail(email.Text.ToString())==false)
                {
                    MessageBox.Show("דואר אלקטרוני לא חוקי");
                    return 0;
                }
                else if(phonenumber.Text.Length!=7)
                {
                    MessageBox.Show("מספר נייד לא תקין");
                    return 0;
                }
            }
            return 1;
        }

        //check if user exist in system
        static bool IdExist(string id)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            
            conn.Open();
           SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) FROM [Users_Table] WHERE ID ='"+id+"'", conn);

            if((int)cmd.ExecuteScalar() > 0)
            {
               return true;
            }

            else
            {

               return false;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            GetUserData();
        }


        //Display datas of users as table
        private void GetUserData()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            try
            {
                conn.Open();
                SqlCommand com = new SqlCommand("SELECT *FROM Users_Table", conn);
                com.Connection = conn;
                SqlDataAdapter datas = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable("Users_Table");
                datas.Fill(dt);
                UsersDataTable.ItemsSource = dt.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }
        //Add user
        private void AddUser(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            int result = 0;

            try
            {
                conn.Open();
               
                    SqlCommand cmd = new SqlCommand("insert into Users_Table (Id,First_Name,Last_Name,BirthDay,Email,Gender,Phone_Number) values('" + id.Text + "','" + firstname.Text + "','" + lastname.Text + "','" + birthday.Text + "','" + email.Text + "','" + gender.Text + "','" + areacode.Text + phonenumber.Text + "')", conn);
                  
               
                if (FillCheck() == 0)
                {

                    GetUserData();
                }
                else if (LegelTextInput() == 0)
                {
                    GetUserData();
                }

                else if(IdExist(id.Text)==true)
                {
                    MessageBox.Show("מספר תעודת זהות קיים במערכת");
                    GetUserData();
                }

                else
                {
                    result = (int)cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("המשתמש הוכנס בהצלחה");
                        GetUserData();
                    }
                    else
                    {
                        MessageBox.Show("לא קיים חיבור למערכת");
                    }
                }
                conn.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("שגיאה");
            }
            finally
            {
                GetUserData();
            }
           
        }
        //Find user as field
        private void Search_User(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            try
            {
                if (comboxsearch.Text == "בחר/י")
                {
                    textboxusersearch = null;
                    GetUserData();
                }
                conn.Open();
                SqlCommand com = new SqlCommand("SELECT *FROM Users_Table WHERE " + ((ComboBoxItem)comboxsearch.SelectedItem).Name + "='" + textboxusersearch.Text + "'", conn);
                com.Connection = conn;
                SqlDataAdapter datas = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable("Users_Table");
                datas.Fill(dt);
                UsersDataTable.ItemsSource = dt.DefaultView;
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }

        //Display user
        private void UserDisplay(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            try
            {
                conn.Open();
                SqlCommand com = new SqlCommand("SELECT *FROM [Users_Table] WHERE ID='" + Id_Show.Text + "'", conn);
                com.Connection = conn;
                SqlDataAdapter datas = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable("Users_Table");
                datas.Fill(dt);
                UserApper.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }
        //Delete user
        private void User_Delete(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            int resual = 0;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Users_Table WHERE Id = '" + Id_Show.Text + "'", conn);
                resual= cmd.ExecuteNonQuery();            
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Id_Show.Text = null;
                UserApper.ItemsSource = null;
                MessageBox.Show("המשתמש הוסר מהמערכת");
            }
            GetUserData();
        }
        //Display user on update screen
        private void User_Display_Update(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            try
            {
                conn.Open();
                SqlCommand com = new SqlCommand("SELECT *FROM [Users_Table] WHERE ID='" + Id_Show_Update.Text + "'", conn);

                SqlDataAdapter datas = new SqlDataAdapter(com);
                System.Data.DataTable dt = new System.Data.DataTable("Users_Table");
                datas.Fill(dt);
                UserApearUpdate.ItemsSource = dt.DefaultView;
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        firstnameupdate.Text = (reader["First_Name"].ToString());
                        lastnameupdate.Text = (reader["Last_Name"].ToString());
                        birthdayupdate.Text = (reader["BirthDay"].ToString());
                        emailupdate.Text = (reader["Email"].ToString());
                        genderupdate.Text = (reader["Gender"].ToString());
                        phonenumberupdate.Text = (reader["Phone_Number"].ToString());                       
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

        }
        //Update user
        private void User_Update(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["Table"].ConnectionString;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [Users_Table] SET First_Name = @firstname,Last_Name=@lastname,BirthDay=@birthday,Email=@email,Gender=@gender,Phone_Number=@phonenumber WHERE ID='" + Id_Show_Update.Text + "'", conn);
             
                cmd.Parameters.AddWithValue("@firstname", firstnameupdate.Text);
                cmd.Parameters.AddWithValue("@lastname", lastnameupdate.Text);
                cmd.Parameters.AddWithValue("@birthday", birthdayupdate.Text);
                cmd.Parameters.AddWithValue("@Email", emailupdate.Text);
                cmd.Parameters.AddWithValue("@gender", genderupdate.Text);
                cmd.Parameters.AddWithValue("@phonenumber", phonenumberupdate.Text);

                cmd.ExecuteNonQuery();

                conn.Close();
              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            finally
            {
                Id_Show_Update.Text = null;
                UserApearUpdate.ItemsSource =null;
                MessageBox.Show("המשתמש עודכן בהצלחה");
            }
            GetUserData();
        }

       
    }
    }
    
    
    
    
    

