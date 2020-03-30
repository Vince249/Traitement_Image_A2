using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Vincent_Poupet
{
    class Program
    {
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


            //header fait à la main
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


        //Problème informatique de Rémi Guillon Bony et de Vincent Poupet
        static void Main(string[] args)
        {
            string nomfichier = "coco.bmp";
            MyImage image = new MyImage(nomfichier);
            MyImage imagenoireetblanc = new MyImage(nomfichier);
            MyImage imagemiroirvertical = new MyImage(nomfichier);
            MyImage imagemiroirhorizontal = new MyImage(nomfichier);
            MyImage imageagrandielargeur = new MyImage(nomfichier);

            MyImage imagerotation = new MyImage(nomfichier);

            MyImage imagebizarre = new MyImage(nomfichier);
            // byte[] tab = { 230, 4, 0, 0 };
            // int a = image.Convertir_Endian_To_Int(tab);
            image.EnregistrerImage("Image de base.bmp");
            image.PassageGris();
            image.EnregistrerImage("Image Grise.bmp");
            imagenoireetblanc.PassageNoiretBlanc();
            imagenoireetblanc.EnregistrerImage("Image Noir et Blanc.bmp");
            imagemiroirvertical.EffetMiroirParRapportALaVerticale();
            imagemiroirvertical.EnregistrerImage("ImageVerticale.bmp");
            imagemiroirhorizontal.EffetMiroirParRapportAHorizontale();

            imagemiroirhorizontal.EnregistrerImage("Image Horizontale.bmp");

            imagebizarre.PassageBizarre();
            imagebizarre.EnregistrerImage("Image bizarre.bmp");


            imagerotation.RotationSensTrigo();
            imagerotation.EnregistrerImage("Test 1 rotation.bmp");




            MyImage imagerotation180 = new MyImage(nomfichier);
            imagerotation180.RotationSensTrigo();
            imagerotation180.RotationSensTrigo();
            imagerotation180.EnregistrerImage("Test 1 rotation180.bmp");


            MyImage imagerotation270 = new MyImage(nomfichier);
            imagerotation270.RotationSensTrigo();
            imagerotation270.RotationSensTrigo();
            imagerotation270.RotationSensTrigo();
            imagerotation270.EnregistrerImage("Test 1 rotation270.bmp");


            imageagrandielargeur.AgrandirImageLargeur(2);
            imageagrandielargeur.EnregistrerImage("Image agrandie largeur x2.bmp");


            MyImage imageagrandiehauteur = new MyImage(nomfichier);
            imageagrandiehauteur.AgrandirImageHauteur(2);
            imageagrandiehauteur.EnregistrerImage("Image agrandie hauteur x2.bmp");


            MyImage imageagrandiex2 = new MyImage(nomfichier);
            imageagrandiex2.AgrandirImageLargeur(7);
            imageagrandiex2.AgrandirImageHauteur(7);
            imageagrandiex2.EnregistrerImage("Image agrandie x7.bmp");


            MyImage imageretreciehauteur = new MyImage(nomfichier);
            imageretreciehauteur.RetrecirImageHauteur(7);
            imageretreciehauteur.EnregistrerImage("Image retrecie hauteur x7.bmp");

            MyImage imageretrecielargeur = new MyImage(nomfichier);
            imageretrecielargeur.RetrecirImageLargeur(7);
            imageretrecielargeur.CorrectionTaillefichierdivisibilitépar4(); //NE PAS OUBLIER CETTE FONCTION
            imageretrecielargeur.EnregistrerImage("Image retrecie largeur x7.bmp");



            MyImage imageflou = new MyImage(nomfichier);
            imageflou.FiltreFlou();
            imageflou.EnregistrerImage("Image flou.bmp");

            //image plus flou que la précedente (on a agrandi la matrice de filtre ATTENTION A LA LAISSER EN TAILLE IMPAIRE)
            MyImage imageflouPlus = new MyImage(nomfichier);
            imageflouPlus.FiltreFlouPlus();
            imageflouPlus.EnregistrerImage("Image flou plus.bmp");

            MyImage imagedectioncontour = new MyImage(nomfichier);
            imagedectioncontour.FiltreDetectionContours();
            imagedectioncontour.EnregistrerImage("Image dectection des contours.bmp");

            MyImage imageRepoussage = new MyImage(nomfichier);
            imageRepoussage.FiltreRepoussage();
            imageRepoussage.EnregistrerImage("Image repoussage.bmp");

            MyImage imageRenforcementdesbords = new MyImage(nomfichier);
            imageRenforcementdesbords.FiltreRenforcementDesBords();
            imageRenforcementdesbords.EnregistrerImage("Image renforcement des bords.bmp");



            //FRACTALE

            //on crée une image noire support pour la fractale et ensuite on ré-enregistre par dessus
            CreeImageBlanche("Fractale.bmp",240,270);
            MyImage Fractale = new MyImage("Fractale.bmp");
            Fractale.AgrandirImageLargeur(10);
            Fractale.AgrandirImageHauteur(10);

            Fractale.Fractale(40);
            Fractale.EnregistrerImage("Fractale.bmp");


            //HISTOGRAMME
            MyImage imagepourhisto = new MyImage("coco.bmp");
            CreeImageBlanche("Histogramme.bmp", imagepourhisto.Largeur,imagepourhisto.Hauteur);
            MyImage Histogramme = new MyImage("Histogramme.bmp");
            Histogramme.Histogramme(imagepourhisto);
            Histogramme.EnregistrerImage("Histogramme.bmp");




            //CODER/DECODER IMAGE DANS IMAGE

            MyImage imageprincipale = new MyImage("lac_en_montagne.bmp");
            MyImage imageacacher = new MyImage("coco.bmp");

            imageprincipale.CoderImageDansUneAutre(imageacacher);
            imageprincipale.EnregistrerImage("Image cachée dans une autre.bmp");

            imageprincipale.DécoderImageDansUneAutre();
            imageprincipale.EnregistrerImage("Image cachée retrouvée dans une autre.bmp");

        }
    }
}

