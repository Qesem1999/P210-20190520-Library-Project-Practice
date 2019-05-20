using Library.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Library
{
    public partial class Book : Form
    {
        List<Models.Author> authors;
        List<Models.Category> categories;


        public Book()
        {
            InitializeComponent();
            FillBooks();
            FillAuthors();
            FillCategoriesCombo();
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
                        authorsFullname += item.Author.Name + " " + item.Author.Surname + ", ";
                    }

                    int orderedBookCount = db.Orders.Where(o => o.BookID == b.Id).Count();
                    dgvBooks.Rows.Add(
                                b.Id,
                                b.Name,
                                authorsFullname,
                                b.Price,
                                b.Category.Name,
                                b.Count - orderedBookCount
                                );
                }


            }
        }

        private void FillAuthors()
        {
            using (LibraryDB db = new LibraryDB())
            {
                lbAuthors.Items.Clear();

                authors = db.Authors.ToList();

                foreach (var item in authors)
                {
                    lbAuthors.Items.Add(item.Name + " " + item.Surname);
                }

            }
        }

        private void FillCategoriesCombo()
        {
            cmbCategories.Items.Clear();
            using (LibraryDB db = new LibraryDB())
            {
                categories = db.Categories.ToList();

                foreach (var c in categories)
                {
                    cmbCategories.Items.Add(c.Name);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int count = Convert.ToInt32(txtCount.Text);

            int selected = cmbCategories.SelectedIndex;

            int catID = categories[selected].Id;


            using (LibraryDB db = new LibraryDB())
            {
                Models.Book book = new Models.Book()
                {
                    Name = name,
                    Price = price,
                    Count = count,
                    CategoryID = catID
                };

                db.Books.Add(book);
                db.SaveChanges();


                foreach (int item in lbAuthors.SelectedIndices)
                {
                    AuthorsBook ab = new AuthorsBook
                    {
                        BookID = book.Id,
                        AuthorID = authors[item].Id
                    };
                    db.AuthorsBooks.Add(ab);
                    db.SaveChanges();
                }
            }
            FillBooks();
        }
    }
}
