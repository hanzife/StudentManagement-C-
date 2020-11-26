using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace FistAppCSharp
{
    public partial class Form1 : Form
    {
       

        public Form1()
        {
            InitializeComponent();

        }
        //Global
        string Pays = "";
        string Ville = "";
        string Choix = "";
        string connetionString = null;
        SqlConnection con;
        SqlCommand cmd;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Variables
            string Nom = textBox1.Text;
            string Prenom = textBox2.Text;
            DateTime Date_Naissance = dateTimePicker1.Value;
            DateTime dateNow = DateTime.Now;
            var age = DateTime.Now.Year - Date_Naissance.Year;
            
            string Telephone = textBox4.Text;
            string Email = textBox5.Text;
            Pays = comboBox2.Text;
            Ville = comboBox3.Text;
            Choix = comboBox1.Text;

            //Indicator Error
            var Err= Controls.OfType<Control>();

            //var ErrChoice = Controls.OfType<Control>();
            //Regex 
            //Phone Numbre
            Regex regPhone = new Regex(@"(\+212|0)([ \-_/]*)(\d[ \-_/]*){9}");
            //is this true?
            bool resPhone = regPhone.IsMatch(Telephone);
            //Email
            Regex regEmail = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            //is this true?
            bool resEmail = regEmail.IsMatch(Email);

            //Check fullname is correct
            Regex regName = new Regex(@"^[a-zA-Z]+$");
            bool resName = regName.IsMatch(Nom);

            Regex regLastname = new Regex(@"^[a-zA-Z]+$");
            bool resLastname = regName.IsMatch(Prenom);

           

            //Conditions - All Values
            /*foreach(var ErrEmpty in Err)
            {
                if (string.IsNullOrWhiteSpace(ErrEmpty.Text))
                {
                    errorProvider1.SetError(ErrEmpty, "Please fill all records");
                }
            }*/
            if (Nom == "" || Prenom== "" || Telephone =="" || Email == "" || Ville=="" || Pays == "" || Choix == "")
            {
                MessageBox.Show("All Feild Must not be Empty!");
            }
            else if (!resName || !resLastname)
            {
                MessageBox.Show("Please verify your fullname is a real name");
            }
            //Check Birthday
            else if ( Date_Naissance >= dateNow || Date_Naissance == dateNow || age < 18)
            {
                MessageBox.Show("Please Insert a correct Date, Must be Older than 18 years old");
            }

            //Validate Phone Number 
            //If not true then send Error
            else if (!resPhone)
            {
                MessageBox.Show("Insert a Correct Phone number (+212 + 9 numbers)");
            }

            //Validate Email Number
            //If not true then send Error
            else if (!resEmail)
            {
                MessageBox.Show("Please Check your Email");
            }
           
            /*if(Pays!= "Maroc" || Pays != "France" || Pays != "England")
            {
                MessageBox.Show("Please Choose one country from the list");   
            }*/

            //no error than Load My DGV
            else
            {
                //Load ListView with a Set of data in one row
                string[] row = { Nom +", " + Prenom, Email, Telephone, age.ToString(), Date_Naissance.ToString(), Pays, Ville, Choix };
                var LV = new ListViewItem(row);
                listView1.Items.Add(LV);
                //MessageBox.Show("hello " + Nom + ", " + Prenom + ", your age is "+age+" you are from "+Pays+", "+Ville+" you choose "+Choix);

                SqlConnection con;
                string connetionString = null;
                connetionString = @"Server=DESKTOP-U1C50GP\SQLEXPRESS;Database=Aprenant;Trusted_Connection=True";
                con = new SqlConnection(connetionString);
                con.Open();

                String query = "Insert into Apprenant(Nom,Prenom,Email,Telephone,Age,birthday,country, City,Choice) values('" + Nom + "','" + Prenom + "','" + Email + "','" + Telephone + "','" + age + "', '" + Date_Naissance + "','" + Pays + "','" + Ville + "','" + Choix + "')";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();

                //Update the Number Of Existing Students 
                CountStudents();

                con.Close();
                MessageBox.Show("Inserted");

            }




        }

        public void RemplirCMB()
        {
            DataTable dt = new DataTable();
            String query = "(select id from Apprenant)";
            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.ExecuteNonQuery();
            // fill the datatable
            dt.Load(cmd.ExecuteReader());

            // set up cbo
            comboBox4.DisplayMember = "Id";
            comboBox4.ValueMember = "Id";
            comboBox4.DataSource = dt;
        }
        //Load my Database
        public void RemplirDGV()
        {
            String query = "(select * from Apprenant)";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            string com = cmd.ExecuteScalar().ToString();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
           


        }

        //Count Number Of Students
        public void CountStudents()
        {
            cmd = new SqlCommand("select count(*) from Apprenant", con);
            lbl_NumTotal.Text = cmd.ExecuteScalar().ToString();
            

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            connetionString = @"Server=DESKTOP-U1C50GP\SQLEXPRESS;Database=Aprenant;Trusted_Connection=True";
            con = new SqlConnection(connetionString);
            try
            {
                con.Open();
                MessageBox.Show("Connection Open ! ");

                //Fill my ComboBox by IDs
                RemplirCMB();
                //Fill My DataGridView
                RemplirDGV();

                CountStudents();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }

            
                
          
                
               

                
                

                
            


            listView1.View = View.Details;
            listView1.Columns.Add("FullName");
            //listView1.Columns.Add("LastName");
            listView1.Columns.Add("Email");
            listView1.Columns.Add("Telephone");
            listView1.Columns.Add("Age");
            listView1.Columns.Add("birthday");
            listView1.Columns.Add("country");
            listView1.Columns.Add("City");
            listView1.Columns.Add("Choice");


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Pays = comboBox2.SelectedIndex;
            //MessageBox.Show(Pays);
            //Is he/she from Maroc? he live in a city from Maroc
            if (Pays == 0)
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                comboBox3.Items.Add("Safi");
                comboBox3.Items.Add("Marrakech");
                comboBox3.Items.Add("Casablanca");
            }
            //Is he/she from France? he live in a city from France
            else if (Pays == 1)
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                comboBox3.Items.Add("Toulous");
                comboBox3.Items.Add("Paris");
                comboBox3.Items.Add("quebec ");
            }
            //Is he/she from England? he live in a city from England
            else if (Pays == 2)
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                comboBox3.Items.Add("London");
                comboBox3.Items.Add("Birmingham");
                comboBox3.Items.Add("Cambridge");
            }
           

        }

        //If he puts a country from his choice, Ask to choose one from the list
        private void comboBox2_Leave(object sender, EventArgs e)
        {
            Pays = comboBox2.Text;
            if (Pays != "Maroc" && Pays != "France" && Pays != "England")
            {
                MessageBox.Show("Please Choose one country from the list");
            }
            
                
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            Pays = comboBox2.Text;
            Ville = comboBox3.Text;
            if (Pays == "Maroc")
            {
                if(Ville != "Safi" && Ville != "Marrakech" && Ville != "Casablanca")
                MessageBox.Show("Please Choose one city from the list");
            }
            else if (Pays == "France")
            {
                if (Ville != "Toulous" && Ville != "Paris" && Ville != "quebec")
                    MessageBox.Show("Please Choose one city from the list");
            }
            else
            {
                if (Ville != "London" && Ville != "Birmingham" && Ville != "Cambridge")
                    MessageBox.Show("Please Choose one city from the list");
            }
        }

        //Search by ID from ComboBox
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get One Id
            string IdStudent = comboBox4.Text;
            String query = "(select * from Apprenant where id ="+IdStudent+")";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            da.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string IdStudent = comboBox4.Text;

            try
            {
                cmd = new SqlCommand("delete from Apprenant where id =" + IdStudent, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Student by Id "+ IdStudent+" is Deleted");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("No Record Deleted");
            }
          


         

        }

        private void Update_Click(object sender, EventArgs e)
        {
            string IdStudent = comboBox4.Text;
            try
            {
                var age = DateTime.Now.Year - dateTimePicker1.Value.Year;
                cmd = new SqlCommand("update Apprenant set nom ='"+ textBox1.Text +"' , Prenom='" + textBox2.Text+ "', Email='" + textBox5.Text+ "', Telephone='" + textBox4.Text+ "', birthday='" + dateTimePicker1.Value  + "',age='"+age+"', country='" + comboBox2.Text+ "', City='" + comboBox3.Text+ "', Choice='" + comboBox1.Text+ "' where id = "+ IdStudent, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Student by Id " + IdStudent + " updated successfully");

               
                
                //cmd = new SqlCommand("select * from Apprenant where id ="+IdStudent);


            }
            catch (Exception ex)
            {
                MessageBox.Show("No Record Deleted");
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //selected row 
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string Idstudent = dataGridView1.SelectedRows[0].Cells[0].Value + string.Empty;
                string name = dataGridView1.SelectedRows[0].Cells[1].Value + string.Empty;
                string prenom = dataGridView1.SelectedRows[0].Cells[2].Value + string.Empty;
                string email = dataGridView1.SelectedRows[0].Cells[3].Value + string.Empty;
                string telephone = dataGridView1.SelectedRows[0].Cells[4].Value + string.Empty;
                //string date_Naissance = dataGridView1.SelectedRows[0].Cells[4].Value + string.Empty;
                //DateTime Date_Naissance = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
                string pays = dataGridView1.SelectedRows[0].Cells[7].Value + string.Empty;
                string ville = dataGridView1.SelectedRows[0].Cells[8].Value + string.Empty;
                string Choix = dataGridView1.SelectedRows[0].Cells[9].Value + string.Empty;

                comboBox4.Text = Idstudent;
                textBox1.Text = name;
                textBox2.Text = prenom;
                textBox5.Text = email;
                textBox4.Text = telephone;
                comboBox2.Text = pays;
                comboBox3.Text = ville;
                comboBox1.Text =  Choix;

                getBirthday(Idstudent);

            }
        }

        //GetDate
        public void getBirthday(string id)
        {
            con.Open();
            cmd = new SqlCommand("select birthday from Apprenant where id =" +id, con);
            cmd.ExecuteNonQuery();
            string com = cmd.ExecuteScalar().ToString();
            con.Close();
            dateTimePicker1.Value = Convert.ToDateTime(com);
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Choice = comboBox5.Text;
                String query = "";
                if (Choice == "All")
                {
                    query = "(select * from Apprenant )";
                }
                else
                {
                    query = "(select * from Apprenant where choice ='" + Choice + "' )";
                }
                con.Open();
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                string com = cmd.ExecuteScalar().ToString();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                da.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("");
            }

        }
    }
}

