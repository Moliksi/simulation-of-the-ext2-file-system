using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
namespace ОперационныеСистемы_приложение_1
{


    public partial class Form1 : Form
    {
        
        public SuperBlock Superblock = new SuperBlock();
        public Catalog catalog = new Catalog();
        public Catalog catalog1 = new Catalog();
        public Group_Descriptors Group_descriptors = new Group_Descriptors();
        public bool[] Block_bitmap = new bool[2880];
        public bool[] Inode_bitmap = new bool[184];
        public Inode[] Table_inode = new Inode[184];
        public Inform_block[] inform_Block = new Inform_block[2880];

        public Form1()
        {
            InitializeComponent();

        }
        ulong poisksvobodbloka()
        {
            bool po = true;
            ulong i = 0;
            while (po)
            {
                if (Block_bitmap[i] == false)
                {
                    po = false;
                    return i;
                }
                i++;
            }
            return 0;
        }
        ulong poisksvoboddescript()
        {
            bool po = true;
            ulong i = 0;
            while (po)
            {
                if (Inode_bitmap[i] == false)
                {
                    po = false;
                    return i;
                }
                i++;
            }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Block_bitmap[0] = true;
            Block_bitmap[1] = true;
            Block_bitmap[2] = true;
            Block_bitmap[3] = true;
            Block_bitmap[4] = true;
            Block_bitmap[5] = true;
            Block_bitmap[6] = true;
            Inode_bitmap[0] = true;
            Inode_bitmap[1] = true;
            listBox1.Items.Add(1);
            listBox1.Items.Add(1);
            listBox1.Items.Add(1);
            listBox1.Items.Add(1);
            listBox1.Items.Add(1);
            listBox1.Items.Add(1);
            listBox1.Items.Add(1);
            Superblock.blocks_per_group = 4096;
            Superblock.block_count = 2880;
            Superblock.block_size = 512;
            Superblock.free_blocks_count = 2880 - 7;
            Superblock.free_inodes_count = 184 - 2;
            Superblock.inodes_count = 184;
            Superblock.inodes_per_group = 184;
            Superblock.reserved = 512;
            Group_descriptors.block_bitmap = 2;
            Group_descriptors.free_blocks_count = 2880 - 7;
            Group_descriptors.free_inodes_count = 184 - 2;
            Group_descriptors.inode_bitmap = 3;
            Group_descriptors.inode_table = 4;
            Group_descriptors.used_dirs_count = 0;
            listBox2.Items.Add(1);
            listBox2.Items.Add(1);
            Inode a = new Inode();
            a.adress = new ulong[1];
            a.file_size = 0;
            Table_inode[0] = a;
            Inode b = new Inode();
            b.adress = new ulong[1];
            b.adress[0] = 1;
            b.file_size = 0;
            Table_inode[1] = b;
            textBox9.Text = listBox1.Items.Count.ToString();
            textBox10.Text = listBox2.Items.Count.ToString();
            catalog.NameCatalog = "С/";
            catalog1.NameCatalog = "С/cat/";
            catalog.NameFile = new List<string>();
            catalog1.NameFile = new List<string>();
            catalog.catalogs = new List<Catalog>();
            catalog.catalogs.Add(catalog1);
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Superblock.free_blocks_count < ulong.Parse(textBox2.Text) / 512)
            {

            }
            else
            {
                ulong b = poisksvobodbloka();
                Superblock.free_inodes_count -= 1;
                Group_descriptors.free_inodes_count -= 1;
                listBox2.Items.Add(1);
                ulong c = poisksvoboddescript();
                Inode_bitmap[c] = true;
                Inode d = new Inode();
                if (c > (ulong)catalog.NameFile.Count)
                {
                    catalog.NameFile.Add(textBox1.Text);
                }
                else
                {
                    catalog.NameFile.Insert((int)c-2, textBox1.Text);
                }
                d.file_size = ulong.Parse(textBox2.Text);
                d.pravo = ulong.Parse(textBox3.Text);
                d.crim = 0;
                Inform_block g = new Inform_block();
                g.Inform = textBox4.Text;
                inform_Block[b] = g;
                ulong h = d.file_size;
                
                    h = (ulong)Math.Ceiling((decimal)d.file_size / 512);
                
                if (h > 2097292)
                {
                    d.adress = new ulong[15];
                    for (int i = 0; i < 12; i++)
                    {
                        d.adress[i] = b;
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                    }

                    d.adress[12] = b;
                    Superblock.free_blocks_count -= 1;
                    Group_descriptors.free_blocks_count -= 1;
                    Block_bitmap[b] = true;
                    listBox1.Items.Add(1);
                    b = poisksvobodbloka();
                    for (int i = 0; i < 128; i++)
                    {
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                    }
                    d.adress[13] = b;
                    Superblock.free_blocks_count -= 1;
                    Group_descriptors.free_blocks_count -= 1;
                    Block_bitmap[b] = true;
                    listBox1.Items.Add(1);
                    b = poisksvobodbloka();
                    for (int i = 0; i < 2097152; i++)
                    {
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                    }
                    d.adress[14] = b;
                    Superblock.free_blocks_count -= 1;
                    Group_descriptors.free_blocks_count -= 1;
                    Block_bitmap[b] = true;
                    listBox1.Items.Add(1);
                    b = poisksvobodbloka();
                    for (int i = 0; i < 268435456; i++)
                    {
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                        if (h == 1)
                            break;
                    }
                }
                else if (h > 140)
                {

                    d.adress = new ulong[14];
                    for (int i = 0; i < 12; i++)
                    {
                        d.adress[i] = b;
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                    }

                    d.adress[12] = b;
                    Superblock.free_blocks_count -= 1;
                    Group_descriptors.free_blocks_count -= 1;
                    Block_bitmap[b] = true;
                    listBox1.Items.Add(1);
                    b = poisksvobodbloka();
                    for (int i = 0; i < 128; i++)
                    {
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                    }
                    d.adress[13] = b;
                    Superblock.free_blocks_count -= 1;
                    Group_descriptors.free_blocks_count -= 1;
                    Block_bitmap[b] = true;
                    listBox1.Items.Add(1);
                    b = poisksvobodbloka();
                    for (int i = 0; i < 2097152; i++)
                    {
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                        if (h == 1)
                            break;
                    }

                }
                else if (h > 12)
                {

                    d.adress = new ulong[13];
                    for (int i = 0; i < 12; i++)
                    {
                        d.adress[i] = b;
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                    }

                    d.adress[12] = b;
                    Superblock.free_blocks_count -= 1;
                    Group_descriptors.free_blocks_count -= 1;
                    Block_bitmap[b] = true;
                    listBox1.Items.Add(1);
                    b = poisksvobodbloka();
                    for (int i = 0; i < 128; i++)
                    {
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                        h--;
                        if (h == 1)
                            break;
                    }
                }
                else
                {

                    d.adress = new ulong[h];
                    for (ulong i = 0; i < h; i++)
                    {
                        d.adress[i] = b;
                        Superblock.free_blocks_count -= 1;
                        Group_descriptors.free_blocks_count -= 1;
                        Block_bitmap[b] = true;
                        listBox1.Items.Add(1);
                        b = poisksvobodbloka();
                    }


                }
                textBox9.Text = listBox1.Items.Count.ToString();
                textBox10.Text = listBox2.Items.Count.ToString();
                Table_inode[c] = d;
                listBox3.Items.Clear();
                foreach (var item in catalog.NameFile)
                {
                    listBox3.Items.Add(item);

                }
                textBox19.Text = Superblock.free_blocks_count.ToString();
                textBox20.Text = Superblock.free_inodes_count.ToString();
            }
        }

        //1440 кб

        public class Inode
        {

            public ulong file_size { get; set; }
            public ulong pravo { get; set; }
            public ulong[] adress { get; set; }
            public ulong crim { get; set; }
        }
        public class Catalog
        {
            public List<string> NameFile { get; set; }
            public string NameCatalog { get; set; }
            public List<Catalog> catalogs { get; set; }
        }

        public class SuperBlock
        {
            public ulong inodes_count { get; set; }
            public ulong block_count { get; set; }
            public ulong block_size { get; set; }
            public ulong free_blocks_count { get; set; }
            public ulong free_inodes_count { get; set; }
            public ulong blocks_per_group { get; set; }
            public ulong reserved { get; set; }
            public ulong inodes_per_group { get; set; }
            /*
            ulong inodes_count = 184; //Число индексных дескрипторов в файловой системе каждый дискриптор занимает 46 блоков
            ulong block_count = 2880;//Число блоков в файловой системе
            ulong block_size = 512; //размер блока
            ulong free_blocks_count = 2880; //Счетчик числа свободных блоков
            ulong free_inodes_count = 184; //Счетчик числа свободных индексных дескрипторов
            ulong blocks_per_group = 4096; //Число блоков в каждой группе блоков
            ulong reserved = 512;//Заполнение до 512 байт
            ulong inodes_per_group = 184; //Число индексных дескрипторов (inodes) в каждой группе блоков
            */

        }

        public class Group_Descriptors
        {
            public ulong block_bitmap { get; set; }
            public ulong inode_bitmap { get; set; }
            public ulong inode_table { get; set; }
            public ulong free_blocks_count { get; set; }
            public ulong free_inodes_count { get; set; }
            public ulong used_dirs_count { get; set; }
            /*ulong block_bitmap = 2; //Адрес блока, содержащего битовую карту блоков (block bitmap) данной группы
                ulong inode_bitmap = 3; //Адрес блока, содержащего битовую карту индексных дескрипторов (inode bitmap) данной группы
                ulong inode_table = 4; //Адрес блока, содержащего таблицу индексных дескрипторов (inode table) данной группы
                ushort free_blocks_count = 2880; //Счетчик числа свободных блоков в данной группе
                ushort free_inodes_count = 184; //Число свободных индексных дескрипторов в данной группе
                ushort used_dirs_count = 0; //Число индексных дескрипторов в данной группе, которые являются каталогами
            */

        }
        public class Inform_block
        {
            public ulong adress { get; set; } //адреса блоков
            public string Inform { get; set; } //информация
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool po = true;
            int i = 0;
            while (po)
            {
                if (int.Parse(textBox14.Text) == 1)
                {
                    if (catalog.NameFile[i] == textBox5.Text)
                    {
                        if (Table_inode[i+2].crim > 0)
                        {
                            catalog.NameFile.RemoveAt(i);
                            listBox3.Items.Clear();

                            foreach (var item in catalog.NameFile)
                            {
                                listBox3.Items.Add(item);

                            }
                            po = false;
                        }
                        else
                        {
                            catalog.NameFile.RemoveAt(i);


                            listBox3.Items.Clear();

                            foreach (var item in catalog.NameFile)
                            {
                                listBox3.Items.Add(item);

                            }
                            for (int j = 0; j < Table_inode[i+2].adress.Length; j++)
                            {
                                Block_bitmap[Table_inode[i+2].adress[j]] = false;
                                listBox1.Items.Remove(1);
                                Superblock.free_blocks_count += 1;
                                Group_descriptors.free_blocks_count += 1;
                            }
                            Table_inode[i+2] = null;
                            Inode_bitmap[i+2] = false;
                            Superblock.free_inodes_count += 1;
                            Group_descriptors.free_inodes_count += 1;
                            listBox2.Items.Remove(1);
                            po = false;
                            textBox9.Text = listBox1.Items.Count.ToString();
                            textBox10.Text = listBox2.Items.Count.ToString();
                        }
                    }
                }
                else
                {
                    if (catalog1.NameFile[i] == textBox5.Text)
                    {
                        if (Table_inode[i+catalog.NameFile.Count+2].crim > 0)
                        {
                            catalog.NameFile.RemoveAt(i);
                            listBox3.Items.Clear();

                            foreach (var item in catalog.NameFile)
                            {
                                listBox3.Items.Add(item);

                            }
                            po = false;
                        }
                        else
                        {
                            catalog1.NameFile.RemoveAt(i);

                            listBox4.Items.Clear();
                            foreach (var item in catalog1.NameFile)
                            {
                                listBox4.Items.Add(item);
                            }
                            for (int j = 0; j < Table_inode[i + catalog.NameFile.Count + 2].adress.Length; j++)
                            {
                                Block_bitmap[Table_inode[i + catalog.NameFile.Count+2].adress[j]] = false;
                                listBox1.Items.Remove(1);
                                Superblock.free_blocks_count += 1;
                                Group_descriptors.free_blocks_count += 1;
                            }
                            Table_inode[i + catalog.NameFile.Count+2] = null;
                            Inode_bitmap[i + catalog.NameFile.Count+2] = false;
                            Superblock.free_inodes_count += 1;
                            Group_descriptors.free_inodes_count += 1;
                            listBox2.Items.Remove(1);
                            po = false;
                            textBox9.Text = listBox1.Items.Count.ToString();
                            textBox10.Text = listBox2.Items.Count.ToString();
                        }
                    }
                }
                i++;
            }
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }




        private void button4_Click(object sender, EventArgs e)
        {
            bool po = true;
            int i = 0;
            ulong b;
            while (po)
            {
                if (int.Parse(textBox15.Text) == 1)
                {
                    if (catalog.NameFile[i] == textBox6.Text)
                    {
                        b = poisksvoboddescript();
                        Inode_bitmap[b] = true;
                        Inode t = new Inode();
                        if (b > (ulong)catalog.NameFile.Count)
                        {
                            catalog.NameFile.Add(textBox6.Text);
                        }
                        else
                        {
                            catalog.NameFile.Insert((int)b-2, textBox6.Text);
                        }                                                            
                        t.file_size = Table_inode[i+2].file_size;
                        t.pravo = Table_inode[i+2].pravo;
                        t.adress = new ulong[Table_inode[i+2].adress.Length];
                        ulong c;
                        for (int j = 0; j < Table_inode[i+2].adress.Length; j++)
                        {
                            c = poisksvobodbloka();
                            Block_bitmap[c] = true;
                            listBox1.Items.Add(1);
                            t.adress[j] = c;
                            Superblock.free_blocks_count -= 1;
                            Group_descriptors.free_blocks_count -= 1;
                        }
                        Table_inode[b] = t;
                        Superblock.free_inodes_count -= 1;
                        Group_descriptors.free_inodes_count -= 1;
                        listBox2.Items.Add(1);
                        po = false;
                        textBox9.Text = listBox1.Items.Count.ToString();
                        textBox10.Text = listBox2.Items.Count.ToString();
                        listBox3.Items.Clear();
                        foreach (var item in catalog.NameFile)
                        {
                            listBox3.Items.Add(item);

                        }
                    }
                    
                }
                else

                {
                    if (catalog1.NameFile[i] == textBox6.Text)
                    {
                        b = poisksvoboddescript();
                        Inode_bitmap[b] = true;
                        Inode t = new Inode();
                        if (b > (ulong)catalog1.NameFile.Count)
                        {
                            catalog1.NameFile.Add(textBox6.Text);
                        }
                        else
                        {
                            catalog1.NameFile.Insert((int)b-2, textBox6.Text);
                        }
                        t.file_size = Table_inode[i + catalog.NameFile.Count+2].file_size;
                        t.pravo = Table_inode[i + catalog.NameFile.Count+2].pravo;
                        t.adress = new ulong[Table_inode[i + catalog.NameFile.Count+2].adress.Length];
                        ulong c;
                        for (int j = 0; j < Table_inode[i + catalog.NameFile.Count+2].adress.Length; j++)
                        {
                            c = poisksvobodbloka();
                            Block_bitmap[c] = true;
                            listBox1.Items.Add(1);
                            t.adress[j] = c;
                            Superblock.free_blocks_count -= 1;
                            Group_descriptors.free_blocks_count -= 1;
                        }
                        Table_inode[b] = t;
                        Superblock.free_inodes_count -= 1;
                        Group_descriptors.free_inodes_count -= 1;
                        listBox2.Items.Add(1);
                        po = false;
                        textBox9.Text = listBox1.Items.Count.ToString();
                        textBox10.Text = listBox2.Items.Count.ToString();
                        listBox4.Items.Clear();
                        foreach (var item in catalog1.NameFile)
                        {
                            listBox4.Items.Add(item);

                        }
                    }
                }
                i++;
            }
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool po = true;
            int i = 0;
            while (po)
            {
                if (int.Parse(textBox7.Text) == 2)
                {
                    if (catalog.NameFile[i] == textBox8.Text)
                    {
                        catalog1.NameFile.Add(catalog.NameFile[i]);
                        catalog.NameFile.RemoveAt(i);
                        listBox4.Items.Clear();
                        listBox3.Items.Clear();
                        foreach (var item in catalog1.NameFile)
                        {
                            listBox4.Items.Add(item);

                        }
                        foreach (var item in catalog.NameFile)
                        {
                            listBox3.Items.Add(item);

                        }
                        po = false;
                    }
                }
                else
                {
                    if (catalog1.NameFile[i] == textBox8.Text)
                    {
                        catalog.NameFile.Add(catalog1.NameFile[i]);
                        catalog1.NameFile.RemoveAt(i);
                        listBox4.Items.Clear();
                        listBox3.Items.Clear();
                        foreach (var item in catalog1.NameFile[i])
                        {
                            listBox4.Items.Add(item);

                        }
                        foreach (var item in catalog.NameFile[i])
                        {
                            listBox3.Items.Add(item);

                        }
                        po = false;
                    }
                }
                i++;
            }
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (true)
            {
                if (int.Parse(textBox16.Text) == 1)
                {
                    if (catalog.NameFile[i] == textBox12.Text)
                    {
                        if(Table_inode[i+2].pravo==1)
                        {
                            catalog.NameFile[i] = textBox13.Text;
                            listBox3.Items.Clear();
                            foreach (var item in catalog.NameFile)
                            {
                                listBox3.Items.Add(item);
                            }                           
                        }                  
                        else
                        {
                            MessageBox.Show("Вы не администратор");

                        }
                        break;
                    }
                }
                else
                {
                    if (catalog1.NameFile[i] == textBox12.Text)
                    {
                        if (Table_inode[i + catalog.NameFile.Count+2].pravo == 1)
                        {
                            catalog1.NameFile[i] = textBox13.Text;
                            listBox4.Items.Clear();
                            foreach (var item in catalog1.NameFile)
                            {
                                listBox4.Items.Add(item);

                            }
                        }
                        break;
                    }
                }
                i++;
            }
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }
        public List<(string,int t)> crimmas = new List<(string, int t)>();
        private void button7_Click(object sender, EventArgs e)
        {
            int i = 0;
            while (true)
            {
                if (int.Parse(textBox11.Text) == 1)
                {
                    if (catalog.NameFile[i] == textBox17.Text)
                    {
                        Table_inode[i+2].crim += 1;
                        crimmas.Add((textBox18.Text,i+2));
                        listBox5.Items.Add(textBox18.Text);
                        break;
                    }
                }
                else
                {
                    if (catalog1.NameFile[i] == textBox17.Text)
                    {
                        Table_inode[i + catalog.NameFile.Count+2].crim += 1;
                        crimmas.Add((textBox18.Text, i + catalog.NameFile.Count+2));
                        listBox5.Items.Add(textBox18.Text);
                        break;
                    }
                }
                i++;
            }
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int i = 0;
            Table_inode[crimmas[i].Item2].crim -= 1;
            while (true)
            {
                if(crimmas[i].Item1==textBox18.Text)
                {
                    listBox5.Items.RemoveAt(i);
                    if(Table_inode[crimmas[i].Item2].crim>0)
                    {
                        crimmas.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        for (int j = 0; j < Table_inode[crimmas[i].Item2].adress.Length; j++)
                        {
                            Block_bitmap[Table_inode[crimmas[i].Item2].adress[j]] = false;
                            listBox1.Items.Remove(1);
                            Superblock.free_blocks_count += 1;
                            Group_descriptors.free_blocks_count += 1;
                        }
                        Table_inode[crimmas[i].Item2] = null;
                        Inode_bitmap[crimmas[i].Item2] = false;
                    //    Superblock.free_inodes_count += 1;
                    //    Group_descriptors.free_inodes_count += 1;
                        listBox2.Items.Remove(1);

                        textBox9.Text = listBox1.Items.Count.ToString();
                        textBox10.Text = listBox2.Items.Count.ToString();
                        crimmas.RemoveAt(i);
                        break;
                    }
                }
            }
            textBox19.Text = Superblock.free_blocks_count.ToString();
            textBox20.Text = Superblock.free_inodes_count.ToString();
        }

        
    }
}