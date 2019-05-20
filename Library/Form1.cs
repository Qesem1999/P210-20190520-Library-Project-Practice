using Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Library
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            FillClients();
            FillBooks();
        }


        private void FillClients()
        {
            dgvClients.Rows.Clear();

            using (LibraryDB db = new LibraryDB())
            {
                List<Models.Client> clients = db.Clients.ToList();

                foreach (var c in clients)
                {
                    dgvClients.Rows.Add(c.Id,
                                        c.Name,
                                        c.Surname,
                                        c.Email,
                                        c.Phone);

                }

            }
        }

        private void FillBooks()
        {
            using (LibraryDB db = new LibraryDB())
            {
                dgvBooks.Rows.Clear();
                List<Models.Book> books = db.Books.ToList();
                foreach (var b in books)
                {
                    var list = b.AuthorsBooks.Where(QQQ =>
                    QQQ.BookID == b.Id
                     ).ToList();

                    string authorsFullname = "";
                    foreach (var item in list)
                    {
                        authorsFullname += item.Author.Name + " " +item.Author.Surname + ", ";
                    }


                    int orderedBookCount = db.Orders.Where(o=>o.BookID== b.Id).Count();

                    dgvBooks.Rows.Add(
                        b.Id,
                        b.Name,
                        authorsFullname,
                        b.Price.ToString("#.##"),
                        b.Category.Name,
                        b.Count - orderedBookCount
                        );
                }


            }
        }

        private void bookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Book book = new Book();
            book.ShowDialog();
        }

        private void authorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Author author = new Author();
            author.ShowDialog();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Orders orders = new Orders();
            orders.ShowDialog();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            category.ShowDialog();
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Client client = new Client();
            client.ShowDialog();
        }

        private void btnSearchClient_Click(object sender, EventArgs e)
        {
            string searchText = txtSearchClient.Text.Trim().ToLower();
            using (LibraryDB db = new LibraryDB())
            {
                List<Models.Client> clientsList = db.Clients.Where(c =>
                    c.Name.ToLower().Contains(searchText) ||
                    c.Surname.ToLower().Contains(searchText) ||
                    c.Email.ToLower().Contains(searchText) ||
                    c.Phone.ToLower().Contains(searchText)
                ).ToList();

                dgvClients.Rows.Clear();

                foreach (var item in clientsList)
                {
                    dgvClients.Rows.Add(item.Id, item.Name, item.Surname, item.Email, item.Phone);
                }

            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            int clientID = (int)dgvClients.SelectedRows[0].Cells[0].Value;
            int bookID = (int)dgvBooks.SelectedRows[0].Cells[0].Value;

            int rowAffected = 0;

            using (LibraryDB db = new LibraryDB())
            {
                Order order = new Order
                {
                    BookID = bookID,
                    ClientID = clientID,
                    OrderDate = DateTime.Now,
                    ReturnDate = DateTime.Now.AddMonths(1)
                };
                db.Orders.Add(order);
                rowAffected = db.SaveChanges();
            }

            if (rowAffected == 0)
            {
                MessageBox.Show("Order cannot be created");
                return;
            }
            MessageBox.Show("Order created successfully");
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            FillClients();
            FillBooks();
        }
    }
}
