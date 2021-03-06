﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Envanter_Takip
{
    public partial class Stok_Azaltma_Sayfasi : Form
    {
        private Envanter_Database.EnvanterContainer dbcontext = new Envanter_Database.EnvanterContainer();
        private Envanter_Database.SUBE sube = new Envanter_Database.SUBE();


        public Stok_Azaltma_Sayfasi(Envanter_Database.SUBE sube)
        {
            this.sube = sube;
            InitializeComponent();
        }

        private void backtoButton_Click(object sender, EventArgs e)
        {
            Stok_Sayfasi ss = new Stok_Sayfasi(sube);
            ss.Show();
            this.Close();
        }

        private void azaltButton_Click(object sender, EventArgs e)
        {
            if ((barkodTextBox.Text.Length==0) || (stokSayisitextBox.Text.Length==0))
            {
                MessageBox.Show("Barkod veya stok sayısı alanları boş bırakılamaz!");
                return;
            }
            int barkod = Convert.ToInt32(barkodTextBox.Text);
            Object[] keys = {barkod, sube.SubeId,sube.KullaniciID };

            var result = dbcontext.URUNSet1.Single(u => u.Barkod == barkod); // değişmemiş hali

            Envanter_Database.URUN urun=dbcontext.URUNSet1.Find(keys); // değişmiş hali
            urun.Stok -= Convert.ToInt32(stokSayisitextBox.Text);

            Envanter_Database.SATIS satim = new Envanter_Database.SATIS();
            satim.Barkod = urun.Barkod;
            satim.Date = DateTime.Now;
            satim.KullaniciID = urun.KullaniciID;
            satim.Miktar = Convert.ToInt32(stokSayisitextBox.Text);
            satim.SubeId = urun.SubeId;
            dbcontext.SATISSet.Add(satim);

            dbcontext.Entry(result).CurrentValues.SetValues(urun);

            dbcontext.SaveChanges();

            Stok_Sayfasi ss = new Stok_Sayfasi(sube);
            this.Close();
            ss.Show();


        }
    }
}
