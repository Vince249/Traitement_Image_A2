using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Convertir_Endian_To_Int()
        {
            byte[] tabtest = { 54, 238, 2, 0 };
            int resultat = 192054;

            int nombretest = Convertir_Endian_To_Int(tabtest);
            Assert.AreEqual(resultat, nombretest);
        }

        [TestMethod]
        public void Convertir_Int_To_Endian()
        {
            int nombretest = 192054;
            byte[] tabresultat = { 54, 238, 2, 0 };

            byte[] tabtest = Convertir_Int_To_Endian(nombretest, 4);

            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(tabresultat[i], tabtest[i]);
            }
        }


        static void CreeImageBlanche(string nom, int largeur, int hauteur)
        {

            int taille = hauteur * largeur * 3 + 54;
            byte[] tab = new byte[taille];

            byte[] tabtaille = new byte[4];
            byte[] tablargeur = new byte[4];
            byte[] tabhauteur = new byte[4];

            tabtaille = Convertir_Int_To_Endian(taille, 4);
            tablargeur = Convertir_Int_To_Endian(largeur, 4);
            tabhauteur = Convertir_Int_To_Endian(hauteur, 4);


            byte[] tabheader = { 66, 77, tabtaille[0], tabtaille[1], tabtaille[2], tabtaille[3], 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, tablargeur[0], tablargeur[1], tablargeur[2], tablargeur[3], tabhauteur[0], tabhauteur[1], tabhauteur[2], tabhauteur[3], 1, 0, 24, 0, 0, 0, 0, 0, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 54; i++)
            {
                tab[i] = tabheader[i];
            }

            for (int i = 54; i < taille; i++)
            {
                tab[i] = 255;
            }

            File.WriteAllBytes(nom, tab);
        }

        static int Convertir_Endian_To_Int(byte[] tab)
        {
            int reponse = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                reponse = reponse + tab[i] * Convert.ToInt32(Math.Pow(256, i));
            }

            return reponse;
        }

        static byte[] Convertir_Int_To_Endian(int valeur, int taille)
        {
            byte[] tableau = new byte[taille];

            for (int i = taille - 1; i >= 0; i--)
            {
                if (valeur / Convert.ToInt32(Math.Pow(256, i)) < 1)
                {
                    tableau[i] = 0;
                }
                else
                {
                    int quotient = valeur / Convert.ToInt32(Math.Pow(256, i));
                    tableau[i] = Convert.ToByte(quotient);
                    valeur = valeur % Convert.ToInt32(Math.Pow(256, i));

                }
            }

            return tableau;

        }


        [TestMethod]
        public void PixelPassageGris()
        {
            Projet_Vincent_Poupet.Pixel pixeltest = new Projet_Vincent_Poupet.Pixel(239, 120, 100);
            Projet_Vincent_Poupet.Pixel pixelresultat = new Projet_Vincent_Poupet.Pixel(153, 153, 153);

            pixeltest.PassageGris();
            Assert.AreEqual(pixeltest.rouge, pixelresultat.rouge);
            Assert.AreEqual(pixeltest.bleu, pixelresultat.bleu);
            Assert.AreEqual(pixeltest.vert, pixelresultat.vert);
        }

        [TestMethod]
        public void PixelPassageNoirEtBlanc()
        {
            Projet_Vincent_Poupet.Pixel pixeltest = new Projet_Vincent_Poupet.Pixel(239, 120, 100);
            Projet_Vincent_Poupet.Pixel pixelresultat = new Projet_Vincent_Poupet.Pixel(255, 255, 255);

            pixeltest.PassageNoirEtBlanc();
            Assert.AreEqual(pixeltest.rouge, pixelresultat.rouge);
            Assert.AreEqual(pixeltest.bleu, pixelresultat.bleu);
            Assert.AreEqual(pixeltest.vert, pixelresultat.vert);
        }

        [TestMethod]
        public void MyImagePassageGris()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            imagetest.PassageGris();

            for (int i = 0; i < imageresultat.Hauteur;i++)
            {
                for (int j = 0; j < imageresultat.Largeur; j++)
                {
                    imageresultat.MatriceDePixels[i, j].PassageGris();
                }
            }

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }

        [TestMethod]
        public void MyImageNoirEtBlanc()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            imagetest.PassageNoiretBlanc();

            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                for (int j = 0; j < imageresultat.Largeur; j++)
                {
                    imageresultat.MatriceDePixels[i, j].PassageNoirEtBlanc();
                }
            }

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }


        [TestMethod]
        public void AgrandirImageLargeur()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("test.bmp");
            Projet_Vincent_Poupet.Pixel[,] matriceimageresultat = new Projet_Vincent_Poupet.Pixel[imagetest.Hauteur, 2 * imagetest.Largeur];

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    int k = 0;
                    while (k < 2)
                    {
                        matriceimageresultat[i, j*2 + k] = imagetest.MatriceDePixels[i, j];
                        k++;
                    }
                }
            }

            imagetest.AgrandirImageLargeur(2);

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, matriceimageresultat[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, matriceimageresultat[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, matriceimageresultat[i, j].vert);
                }
            }
        }


        [TestMethod]
        public void AgrandirImageHauteur()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("test.bmp");
            Projet_Vincent_Poupet.Pixel[,] matriceimageresultat = new Projet_Vincent_Poupet.Pixel[2 * imagetest.Hauteur, imagetest.Largeur];

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    int k = 0;
                    while (k < 2)
                    {
                        matriceimageresultat[i * 2 + k, j] = imagetest.MatriceDePixels[i, j];
                        k++;
                    }
                }
            }

            imagetest.AgrandirImageHauteur(2);

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, matriceimageresultat[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, matriceimageresultat[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, matriceimageresultat[i, j].vert);
                }
            }
        }

        //Pour rétrécir, on fait la même chose dans le sens inverse 


        [TestMethod]
        public void CoderImageDansUneAutre()
        {
            Projet_Vincent_Poupet.MyImage imagetestprincipale = new Projet_Vincent_Poupet.MyImage("lac_en_montagne.bmp");
            Projet_Vincent_Poupet.MyImage imagetestacacher = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            Projet_Vincent_Poupet.MyImage imageresultatprincipale = new Projet_Vincent_Poupet.MyImage("lac_en_montagne.bmp");
            Projet_Vincent_Poupet.MyImage imageresultatacacher = new Projet_Vincent_Poupet.MyImage("coco.bmp"); 

            int[] tabrougeimageprincipale = new int[imageresultatprincipale.Hauteur * imageresultatprincipale.Largeur];
            int[] tabbleuimageprincipale = new int[imageresultatprincipale.Hauteur * imageresultatprincipale.Largeur];
            int[] tabvertimageprincipale = new int[imageresultatprincipale.Hauteur * imageresultatprincipale.Largeur];

            int[] tabrougeimagecachée = new int[imageresultatacacher.Hauteur * imageresultatacacher.Largeur];
            int[] tabbleuimagecachée = new int[imageresultatacacher.Hauteur * imageresultatacacher.Largeur];
            int[] tabvertimagecachée = new int[imageresultatacacher.Hauteur * imageresultatacacher.Largeur];

            int[] tabrougeresultat = new int[imageresultatacacher.Hauteur * imageresultatacacher.Largeur];
            int[] tabbleuresultat = new int[imageresultatacacher.Hauteur * imageresultatacacher.Largeur];
            int[] tabvertresultat = new int[imageresultatacacher.Hauteur * imageresultatacacher.Largeur];

            int k = 0;
            for (int i = 0; i < imageresultatacacher.Hauteur; i++)
            {
                for (int j = 0; j < imageresultatacacher.Largeur; j++)
                {
                    tabrougeimageprincipale[k] = imageresultatprincipale.MatriceDePixels[i, j].rouge & 240; 
                    tabbleuimageprincipale[k] = imageresultatprincipale.MatriceDePixels[i, j].bleu & 240;
                    tabvertimageprincipale[k] = imageresultatprincipale.MatriceDePixels[i, j].vert & 240;

                    tabrougeimagecachée[k] = imageresultatacacher.MatriceDePixels[i, j].rouge & 240;
                    tabrougeimagecachée[k] = tabrougeimagecachée[k] >> 4; 
                    tabbleuimagecachée[k] = imageresultatacacher.MatriceDePixels[i, j].bleu & 240;
                    tabbleuimagecachée[k] = tabbleuimagecachée[k] >> 4;
                    tabvertimagecachée[k] = imageresultatacacher.MatriceDePixels[i, j].vert & 240;
                    tabvertimagecachée[k] = tabvertimagecachée[k] >> 4;


                    tabrougeresultat[k] = tabrougeimageprincipale[k] | tabrougeimagecachée[k]; 
                    tabbleuresultat[k] = tabbleuimageprincipale[k] | tabbleuimagecachée[k];
                    tabvertresultat[k] = tabvertimageprincipale[k] | tabvertimagecachée[k];

                    k++;
                }
            }

            imagetestprincipale.CoderImageDansUneAutre(imagetestacacher);

            k = 0;

            for (int i = 0; i < imageresultatacacher.Hauteur; i++)
            {
                for (int j = 0; j < imageresultatacacher.Largeur; j++)
                {
                    Assert.AreEqual(imagetestprincipale.MatriceDePixels[i, j].rouge, tabrougeresultat[k]);
                    Assert.AreEqual(imagetestprincipale.MatriceDePixels[i, j].bleu, tabbleuresultat[k]);
                    Assert.AreEqual(imagetestprincipale.MatriceDePixels[i, j].vert, tabvertresultat[k]);
                    k++;
                }
            }
        }


        [TestMethod]
        public void DecoderImageDansUneAutre()
        {
            Projet_Vincent_Poupet.MyImage imagetestprincipale = new Projet_Vincent_Poupet.MyImage("lac_en_montagne.bmp");
            Projet_Vincent_Poupet.MyImage imagetestacacher = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            Projet_Vincent_Poupet.MyImage imageresultatprincipale = new Projet_Vincent_Poupet.MyImage("lac_en_montagne.bmp");
            Projet_Vincent_Poupet.MyImage imageresultatacacher = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int[] tabrougeresultatacacher = new int[imageresultatprincipale.Hauteur * imageresultatprincipale.Largeur];
            int[] tabbleuresultatacacher = new int[imageresultatprincipale.Hauteur * imageresultatprincipale.Largeur];
            int[] tabvertresultatacacher = new int[imageresultatprincipale.Hauteur * imageresultatprincipale.Largeur];

            int k = 0;
            for (int i = 0; i < imageresultatprincipale.Hauteur; i++)
            {
                for (int j = 0; j < imageresultatprincipale.Largeur; j++)
                {
                    tabrougeresultatacacher[k] = imageresultatprincipale.MatriceDePixels[i, j].rouge & 15;
                    tabrougeresultatacacher[k] = tabrougeresultatacacher[k] << 4;
                    tabbleuresultatacacher[k] = imageresultatprincipale.MatriceDePixels[i, j].bleu & 15;
                    tabbleuresultatacacher[k] = tabbleuresultatacacher[k] << 4;
                    tabvertresultatacacher[k] = imageresultatprincipale.MatriceDePixels[i, j].vert & 15; 
                    tabvertresultatacacher[k] = tabvertresultatacacher[k] << 4;

                    tabrougeresultatacacher[k] = tabrougeresultatacacher[k] & 255; 
                    tabbleuresultatacacher[k] = tabbleuresultatacacher[k] & 255;
                    tabvertresultatacacher[k] = tabvertresultatacacher[k] & 255;

                    k++;
                }
            }

            imagetestprincipale.DécoderImageDansUneAutre();

            k = 0;

            for (int i = 0; i < imageresultatprincipale.Hauteur; i++)
            {
                for (int j = 0; j < imageresultatprincipale.Largeur; j++)
                {
                    Assert.AreEqual(imagetestprincipale.MatriceDePixels[i, j].rouge, tabrougeresultatacacher[k]);
                    Assert.AreEqual(imagetestprincipale.MatriceDePixels[i, j].bleu, tabbleuresultatacacher[k]);
                    Assert.AreEqual(imagetestprincipale.MatriceDePixels[i, j].vert, tabvertresultatacacher[k]);
                    k++;
                }
            }
        }


        [TestMethod]
        public void Histogramme()
        {
            Projet_Vincent_Poupet.MyImage imagepourhisto = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            CreeImageBlanche("histogramme.bmp", imagepourhisto.Largeur, imagepourhisto.Hauteur);
            CreeImageBlanche("histogramme2.bmp", imagepourhisto.Largeur, imagepourhisto.Hauteur);
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("histogramme.bmp");
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("histogramme2.bmp");

            for (int i = 0; i < imagepourhisto.Largeur; i++)
            {
                int sommevert = 0;
                int sommerouge = 0;
                int sommebleu = 0;

                for (int j = 0; j < imagepourhisto.Hauteur; j++)
                {
                    sommebleu = sommebleu + imagepourhisto.MatriceDePixels[j, i].bleu;
                    sommevert = sommevert + imagepourhisto.MatriceDePixels[j, i].vert;
                    sommerouge = sommerouge + imagepourhisto.MatriceDePixels[j, i].rouge;
                }

                int nbbleu = (sommebleu / 255) / 3;
                int nbvert = (sommevert / 255) / 3;
                int nbrouge = (sommerouge / 255) / 3;

                for (int k = 0; k < imagepourhisto.Hauteur; k++)
                {

                    if (nbbleu > 0)
                    {
                        imageresultat.MatriceDePixels[k, i] = new Projet_Vincent_Poupet.Pixel(0, 0, 255);
                        nbbleu--;
                    }
                    else
                    {
                        if (nbrouge > 0)
                        {
                            imageresultat.MatriceDePixels[k, i] = new Projet_Vincent_Poupet.Pixel(255, 0, 0);
                            nbrouge--;
                        }
                        else
                        {
                            if (nbvert > 0)
                            {
                                imageresultat.MatriceDePixels[k, i] = new Projet_Vincent_Poupet.Pixel(0, 255, 0);
                                nbvert--;
                            }
                        }
                    }

                }
            }

            imagetest.Histogramme(imagepourhisto);

            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                for (int j = 0; j < imageresultat.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }

        }


        [TestMethod]
        public void Rotation()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.Pixel[,] matricetournée = new Projet_Vincent_Poupet.Pixel[imagetest.Largeur, imagetest.Hauteur];


            for (int j = 0; j < imagetest.Largeur; j++)
            {
                for (int i = 0; i < imagetest.Hauteur; i++)
                {
                    matricetournée[j, imagetest.Hauteur - i - 1] = imagetest.MatriceDePixels[i, j];
                }
            }

            imagetest.RotationSensTrigo();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, matricetournée[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, matricetournée[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, matricetournée[i, j].vert);
                }
            }
        }

        [TestMethod]
        public void EffetMiroirParRapportALaVerticale()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int milieu;
            if (imageresultat.Largeur % 2 == 0)
            {
                milieu = imageresultat.Largeur / 2;
            }
            else
            {
                milieu = (imageresultat.Largeur - 1) / 2;
            }


            for (int i = 0; i < milieu; i++)
            {
                for (int j = 0; j < imageresultat.Hauteur; j++)
                {
                    Projet_Vincent_Poupet.Pixel Clone = new Projet_Vincent_Poupet.Pixel(imageresultat.MatriceDePixels[j, i].rouge, imageresultat.MatriceDePixels[j, i].vert, imageresultat.MatriceDePixels[j, i].bleu);
                    imageresultat.MatriceDePixels[j, i] = imageresultat.MatriceDePixels[j, imageresultat.Largeur - i - 1];
                    imageresultat.MatriceDePixels[j, imageresultat.Largeur - i - 1] = Clone;
                }
            }

            imagetest.EffetMiroirParRapportALaVerticale();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }

        [TestMethod]
        public void EffetMiroirParRapportAHorizontale()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int milieu;
            if (imageresultat.Hauteur % 2 == 0)
            {
                milieu = imageresultat.Hauteur / 2;
            }
            else
            {
                milieu = (imageresultat.Hauteur - 1) / 2;
            }


            for (int i = 0; i < imageresultat.Largeur; i++)
            {
                for (int j = 0; j < milieu; j++)
                {
                    Projet_Vincent_Poupet.Pixel Clone = new Projet_Vincent_Poupet.Pixel(imageresultat.MatriceDePixels[j, i].rouge, imageresultat.MatriceDePixels[j, i].vert, imageresultat.MatriceDePixels[j, i].bleu);
                    imageresultat.MatriceDePixels[j, i] = imageresultat.MatriceDePixels[imageresultat.Hauteur - j - 1, i];
                    imageresultat.MatriceDePixels[imageresultat.Hauteur - j - 1, i] = Clone;

                }
            }

            imagetest.EffetMiroirParRapportAHorizontale();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }


        [TestMethod]
        public void FiltreFlouPlus()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int[,] matrice = new int[,] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } };

            int m = 0;
            int n = 0;

            for (int i = 2; i < imageresultat.Hauteur - 2; i++) 
            {
                for (int j = 2; j < imageresultat.Largeur - 2; j++)
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 2; k <= i + 2; k++)
                    {
                        for (int l = j - 2; l <= j + 2; l++)
                        {
                            totalrouge = totalrouge + imageresultat.MatriceDePixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + imageresultat.MatriceDePixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + imageresultat.MatriceDePixels[k, l].vert * matrice[m, n];
                            n++;
                        }
                        m++;
                        n = 0;
                    }
                    m = 0;


                    imageresultat.MatriceDePixels[i, j].rouge = totalrouge / 25; 
                    imageresultat.MatriceDePixels[i, j].bleu = totalbleu / 25;
                    imageresultat.MatriceDePixels[i, j].vert = totalvert / 25;
                }
            }

            imagetest.FiltreFlouPlus();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }


        [TestMethod]
        public void FiltreRepoussage()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int[,] matrice = new int[,] { { 0, 1, 2 }, { -1, 1, 1 }, { -2, -1, 0 } };

            int m = 0;
            int n = 0;
            int[] tableaurouge = new int[imageresultat.Hauteur * imageresultat.Largeur];
            int[] tableaubleu = new int[imageresultat.Hauteur * imageresultat.Largeur];
            int[] tableauvert = new int[imageresultat.Hauteur * imageresultat.Largeur];

            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                tableaurouge[i] = 0;
                tableaubleu[i] = 0;
                tableauvert[i] = 0;
            }

            int g = 0;
            for (int i = 1; i < imageresultat.Hauteur - 1; i++) 
            {
                for (int j = 1; j < imageresultat.Largeur - 1; j++)
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + imageresultat.MatriceDePixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + imageresultat.MatriceDePixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + imageresultat.MatriceDePixels[k, l].vert * matrice[m, n];
                            n++;
                        }
                        m++;
                        n = 0;
                    }
                    m = 0;

                    if (totalrouge > 255)
                    {
                        totalrouge = 255;
                    }
                    if (totalbleu > 255)
                    {
                        totalbleu = 255;
                    }
                    if (totalvert > 255)
                    {
                        totalvert = 255;
                    }
                    if (totalrouge < 0)
                    {
                        totalrouge = 0;
                    }
                    if (totalbleu < 0)
                    {
                        totalbleu = 0;
                    }
                    if (totalvert < 0)
                    {
                        totalvert = 0;
                    }

                    tableaurouge[g] = totalrouge;
                    tableaubleu[g] = totalbleu;
                    tableauvert[g] = totalvert;
                    g++;
                }
            }

            g = 0;
            for (int i = 1; i < imageresultat.Hauteur - 1; i++) 
            {
                for (int j = 1; j < imageresultat.Largeur - 1; j++) 
                {
                    imageresultat.MatriceDePixels[i, j].rouge = tableaurouge[g];
                    imageresultat.MatriceDePixels[i, j].bleu = tableaubleu[g];
                    imageresultat.MatriceDePixels[i, j].vert = tableauvert[g];
                    g++;
                }
            }

            imagetest.FiltreRepoussage();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }

        [TestMethod]
        public void FiltreDetectionContours()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int[,] matrice = new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } }; 

            int m = 0;
            int n = 0;
            int[] tableaurouge = new int[imageresultat.Largeur * imageresultat.Hauteur];
            int[] tableaubleu = new int[imageresultat.Largeur * imageresultat.Hauteur];
            int[] tableauvert = new int[imageresultat.Largeur * imageresultat.Hauteur];

            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                tableaurouge[i] = 0;
                tableaubleu[i] = 0;
                tableauvert[i] = 0;
            }

            int g = 0;
            for (int i = 1; i < imageresultat.Hauteur - 1; i++) 
            {
                for (int j = 1; j < imageresultat.Largeur - 1; j++) 
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + imageresultat.MatriceDePixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + imageresultat.MatriceDePixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + imageresultat.MatriceDePixels[k, l].vert * matrice[m, n];
                            n++;
                        }
                        m++;
                        n = 0;
                    }
                    m = 0;

                    if (totalrouge > 255)
                    {
                        totalrouge = 255;
                    }
                    if (totalbleu > 255)
                    {
                        totalbleu = 255;
                    }
                    if (totalvert > 255)
                    {
                        totalvert = 255;
                    }
                    if (totalrouge < 0)
                    {
                        totalrouge = 0;
                    }
                    if (totalbleu < 0)
                    {
                        totalbleu = 0;
                    }
                    if (totalvert < 0)
                    {
                        totalvert = 0;
                    }

                    tableaurouge[g] = totalrouge;
                    tableaubleu[g] = totalbleu;
                    tableauvert[g] = totalvert;
                    g++;
                }
            }

            g = 0;
            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                for (int j = 0; j < imageresultat.Largeur; j++) 
                {
                   
                    if (i == 0 || i == imageresultat.Hauteur - 1 || j == 0 || j == imageresultat.Largeur - 1)
                    {
                        imageresultat.MatriceDePixels[i, j].rouge = 0;
                        imageresultat.MatriceDePixels[i, j].bleu = 0;
                        imageresultat.MatriceDePixels[i, j].vert = 0;
                    }
                    else
                    {
                        imageresultat.MatriceDePixels[i, j].rouge = tableaurouge[g];
                        imageresultat.MatriceDePixels[i, j].bleu = tableaubleu[g];
                        imageresultat.MatriceDePixels[i, j].vert = tableauvert[g];
                        g++;
                    }
                }
            }

            imagetest.FiltreDetectionContours();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }


        [TestMethod]
        public void FiltreRenforcementDesBords()
        {
            Projet_Vincent_Poupet.MyImage imagetest = new Projet_Vincent_Poupet.MyImage("coco.bmp");
            Projet_Vincent_Poupet.MyImage imageresultat = new Projet_Vincent_Poupet.MyImage("coco.bmp");

            int[,] matrice = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };

            int m = 0;
            int n = 0;
            int[] tableaurouge = new int[imageresultat.Largeur * imageresultat.Hauteur];
            int[] tableaubleu = new int[imageresultat.Largeur * imageresultat.Hauteur];
            int[] tableauvert = new int[imageresultat.Largeur * imageresultat.Hauteur];

            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                tableaurouge[i] = 0;
                tableaubleu[i] = 0;
                tableauvert[i] = 0;
            }

            int g = 0;
           
            for (int i = 1; i < imageresultat.Hauteur - 1; i++) 
            {
                for (int j = 1; j < imageresultat.Largeur - 1; j++) 
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + imageresultat.MatriceDePixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + imageresultat.MatriceDePixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + imageresultat.MatriceDePixels[k, l].vert * matrice[m, n];
                            n++;
                        }
                        m++;
                        n = 0;
                    }
                    m = 0;

                    if (totalrouge > 255)
                    {
                        totalrouge = 255;
                    }
                    if (totalbleu > 255)
                    {
                        totalbleu = 255;
                    }
                    if (totalvert > 255)
                    {
                        totalvert = 255;
                    }
                    if (totalrouge < 0)
                    {
                        totalrouge = 0;
                    }
                    if (totalbleu < 0)
                    {
                        totalbleu = 0;
                    }
                    if (totalvert < 0)
                    {
                        totalvert = 0;
                    }

                    tableaurouge[g] = totalrouge;
                    tableaubleu[g] = totalbleu;
                    tableauvert[g] = totalvert;
                    g++;
                }
            }

            g = 0;
            for (int i = 0; i < imageresultat.Hauteur; i++)
            {
                for (int j = 0; j < imageresultat.Largeur; j++) 
                {
                    if (i == 0 || i == imageresultat.Hauteur - 1 || j == 0 || j == imageresultat.Largeur - 1)
                    {
                        imageresultat.MatriceDePixels[i, j].rouge = 0;
                        imageresultat.MatriceDePixels[i, j].bleu = 0;
                        imageresultat.MatriceDePixels[i, j].vert = 0;
                    }
                    else
                    {
                        imageresultat.MatriceDePixels[i, j].rouge = tableaurouge[g];
                        imageresultat.MatriceDePixels[i, j].bleu = tableaubleu[g];
                        imageresultat.MatriceDePixels[i, j].vert = tableauvert[g];
                        g++;
                    }
                }
            }

            imagetest.FiltreRenforcementDesBords();

            for (int i = 0; i < imagetest.Hauteur; i++)
            {
                for (int j = 0; j < imagetest.Largeur; j++)
                {
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].rouge, imageresultat.MatriceDePixels[i, j].rouge);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].bleu, imageresultat.MatriceDePixels[i, j].bleu);
                    Assert.AreEqual(imagetest.MatriceDePixels[i, j].vert, imageresultat.MatriceDePixels[i, j].vert);
                }
            }
        }

    }
}
