using DbForm.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace DbForm
{
    public partial class Form1 : Form
    {
        private BTree bTree;

        public Form1()
        {
            bTree = LoadOrInitializeTree("btree.json");
            InitializeComponent();
            InitializeDatabaseTable();
        }

        private void InitializeDatabaseTable()
        {
            dbTable.Rows.Clear();
            var allKeys = bTree.GetAll();

            foreach (var item in allKeys)
            {
                dbTable.Rows.Add(item.Id, item.Name);
            }
        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            var nextId = dbTable.Rows.Count > 0
                ? Convert.ToInt32(dbTable.Rows[dbTable.Rows.Count - 1].Cells[0].Value) + 1
                : 1;

            var model = new SampleModel
            {
                Id = nextId,
                Name = "[null]"
            };

            bTree.Insert(model);
            dbTable.Rows.Add(model.Id, model.Name);
        }

        private void DeleteRowButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedRow in dbTable.SelectedRows)
            {
                if (!selectedRow.IsNewRow)
                {
                    var id = Convert.ToInt32(selectedRow.Cells[0].Value);
                    dbTable.Rows.RemoveAt(selectedRow.Index);
                    bTree.Delete(id);
                }
            }
        }

        private BTree LoadOrInitializeTree(string filename)
        {
            try
            {
                return LoadTreeFromFile(filename);
            }
            catch (FileNotFoundException)
            {
                return CreateDefaultTree();
            }
        }

        private static BTree LoadTreeFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<BTree>(json,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
        }

        private static BTree CreateDefaultTree()
        {
            var tree = new BTree(t: 10);
            for (int i = 1; i < 10000; i++)
            {
                tree.Insert(new SampleModel
                {
                    Id = i,
                    Name = $"Name {i}"
                });
            }
            return tree;
        }

        private static void SaveTreeToFile(BTree tree, string filePath)
        {
            string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });

            File.WriteAllText(filePath, json);
        }

        private void HandleDefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Id"].Value = dbTable.Rows.Count;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(searchInput.Text, out int searchId))
            {
                var result = bTree.Search(searchId, true);
                var model = result.Item3;

                if (result.Item1)
                {
                    MessageBox.Show($"Id: {model.Id}, Name: {model.Name}. Comparisons: {result.Item2}");
                }
                else
                {
                    MessageBox.Show("Not found");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void SaveTreeAndExportButton_Click(object sender, EventArgs e)
        {
            SaveTreeToFile(bTree, "btree.json");
            bTree.ExportToDot("btree.dot");
        }

        private void HandleCellEditEnd(object sender, DataGridViewCellEventArgs e)
        {
            var editedValue = dbTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            bTree.Update(e.RowIndex + 1, editedValue.ToString());
        }
    }
}
