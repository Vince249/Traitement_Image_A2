using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Projet_Vincent_Poupet
{
    //Problème informatique de Rémi Guillon Bony et de Vincent Poupet
    /// <summary>
    /// Classe qui remplace en quelques sortes la classe Bitmap
    /// </summary>
    public class MyImage
    {


        private string type;
        private int taillefichier;
        private int tailleoffset;
        private int hauteur;
        private int largeur;
        private int nombrebitsparcouleur;
        private Pixel[,] matricedepixels;
        private byte[] Header = new byte[54];

        /// <summary>
        /// Constructeur de la classe MyImage
        /// </summary>
        /// <param name="NomFichierAvecExtension">
        /// Contient le chemin (absolu avec WPF, relatif dans les tests qu'on a pu faire)
        /// </param>
        public MyImage(string NomFichierAvecExtension)
        {
            //HEADER


            int rang = 0;
            for (int i = 0; i < NomFichierAvecExtension.Length; i++)
            {
                if (NomFichierAvecExtension[i] == '.')
                {
                    rang = i;
                }
            }
            string format = "";
            for (int i = rang + 1; i < NomFichierAvecExtension.Length; i++)
            {
                format = format + NomFichierAvecExtension[i];
            }

            //ici on connaît le format du fichier
            List<byte> Listebyte = new List<byte>();


            if (format == "csv") //si c'est un csv, on va utiliser une stream pour le lire puis split la liste
            {

                StreamReader flux = new StreamReader(NomFichierAvecExtension);
                string ligne = "";
                List<string> liste = new List<string>();
                while ((ligne = flux.ReadLine()) != null)
                {
                    liste.Add(ligne);
                }




                List<byte> liste2 = new List<byte>();
                for (int i = 0; i < liste.Count; i++)
                {

                    string[] tab = liste[i].Split(';');
                    for (int k = 0; k < tab.Length; k++)
                    {
                        if (tab[k] != "")
                        {
                            liste2.Add(Convert.ToByte(tab[k]));
                        }
                    }
                }


                Listebyte = liste2;
                flux.Close();
            }




            if (format == "bmp")
            {
                byte[] tab = File.ReadAllBytes(NomFichierAvecExtension);
                Listebyte = tab.ToList();

            }

            byte[] tableaubyte = Listebyte.ToArray();

            for (int z = 0; z < 54; z++)
            {
                this.Header[z] = tableaubyte[z];
            }

            this.type = format;
            byte[] TabTailleFichier = { tableaubyte[2], tableaubyte[3], tableaubyte[4], tableaubyte[5] };
            this.taillefichier = Convertir_Endian_To_Int(TabTailleFichier);

            byte[] TabTailleOffset = { tableaubyte[10], tableaubyte[11], tableaubyte[12], tableaubyte[13] };
            this.tailleoffset = Convertir_Endian_To_Int(TabTailleOffset);

            //HEADER INFO

            byte[] TabLargeur = { tableaubyte[18], tableaubyte[19], tableaubyte[20], tableaubyte[21] };
            this.largeur = Convertir_Endian_To_Int(TabLargeur);



            byte[] TabHauteur = { tableaubyte[22], tableaubyte[23], tableaubyte[24], tableaubyte[25] };
            this.hauteur = Convertir_Endian_To_Int(TabHauteur);

            byte[] TabNombreBitsCouleur = { tableaubyte[28], tableaubyte[29] };
            this.nombrebitsparcouleur = Convertir_Endian_To_Int(TabNombreBitsCouleur);


            //IMAGE

            int j = 0;
            Pixel[] tableauintermédiaire = new Pixel[this.hauteur * this.largeur]; //on crée le tableau de Pixel qui fera une taille suffisante pour acceuillir tous les Pixels, un Pixel renferme 3 bytes
            for (int i = 54; i < this.taillefichier - 2; i = i + 3)
            {
                tableauintermédiaire[j] = new Pixel(tableaubyte[i], tableaubyte[i + 1], tableaubyte[i + 2]);
                j++;
            }

            //le tableau de Pixels est bien créé et rempli
            int compteur = 0;
            this.matricedepixels = new Pixel[this.hauteur, this.largeur];
            for (int k = 0; k < this.hauteur; k++)
            {
                for (int l = 0; l < this.largeur; l++)
                {
                    this.matricedepixels[k, l] = tableauintermédiaire[compteur];
                    compteur++;
                }
            }


        }

        /// <summary>
        /// Permet d'accéder à la valeur de la largeur de l'instance
        /// </summary>
        public int Largeur
        {
            get
            {
                return this.largeur;
            }
        }

        /// <summary>
        /// Permet d'accéder à la valeur de la hauteur de l'instance
        /// </summary>
        public int Hauteur
        {
            get
            {
                return this.hauteur;
            }
        }

        /// <summary>
        /// Permet d'accéder à la matrice de pixels de l'instance
        /// </summary>
        public Pixel[,] MatriceDePixels
        {
            get
            {
                return this.matricedepixels;
            }
        }

        /// <summary>
        /// Méthode qui passe l'image en nuance de gris
        /// </summary>
        public void PassageGris()
        {

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    this.matricedepixels[i, j].PassageGris();
                }
            }
        }

        /// <summary>
        /// Cette méthode n'a pas d'intérêt, il s'agit d'une méthode ratée que nous avons conservé car nous appréciions son effet
        /// </summary>
        public void PassageBizarre()
        {

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    this.matricedepixels[i, j].PassageBizarre();

                }
            }
        }

        /// <summary>
        /// Cette méthode permet d'agrandir l'image en hauteur en fonction d'un coefficient donné
        /// </summary>
        /// <param name="coefficient">
        /// Coefficient d'agrandissement entier qui sera compris entre 1 et 10 dans le WPF
        /// </param>
        public void AgrandirImageLargeur(int coefficient)
        {
            Pixel[,] matriceagrandie = new Pixel[this.hauteur, this.largeur * coefficient];

            for (int hauteur = 0; hauteur < this.hauteur; hauteur++)
            {
                for (int largeur = 0; largeur < this.largeur; largeur++)
                {
                    int k = 0;
                    while (k < coefficient)
                    {
                        matriceagrandie[hauteur, largeur * coefficient + k] = this.matricedepixels[hauteur, largeur];
                        k++;
                    }
                }
            }
            this.matricedepixels = null;
            this.largeur = this.largeur * coefficient;
            this.matricedepixels = new Pixel[this.hauteur, this.largeur];

            for (int i = 0; i < this.largeur; i++)
            {
                for (int j = 0; j < this.hauteur; j++)
                {
                    this.matricedepixels[j, i] = matriceagrandie[j, i];
                }
            }



        }

        /// <summary>
        /// Cette méthode permet d'agrandir l'image en largeur en fonction d'un coefficient donné
        /// </summary>
        /// <param name="coefficient">
        /// Coefficient d'agrandissement entier qui sera compris entre 1 et 10 dans le WPF
        /// </param>
        public void AgrandirImageHauteur(int coefficient)
        {

            Pixel[,] matriceagrandie = new Pixel[this.hauteur * coefficient, this.largeur];

            for (int hauteur = 0; hauteur < this.hauteur; hauteur++)
            {
                for (int largeur = 0; largeur < this.largeur; largeur++)
                {
                    int k = 0;
                    while (k < coefficient)
                    {
                        matriceagrandie[hauteur * coefficient + k, largeur] = this.matricedepixels[hauteur, largeur];
                        k++;
                    }
                }
            }
            this.matricedepixels = null;
            this.hauteur = this.hauteur * coefficient;
            this.matricedepixels = new Pixel[this.hauteur, this.largeur];

            for (int i = 0; i < this.largeur; i++)
            {
                for (int j = 0; j < this.hauteur; j++)
                {
                    this.matricedepixels[j, i] = matriceagrandie[j, i];
                }
            }
        }

        /// <summary>
        /// Cette méthode permet de réduire l'image en hauteur en fonction d'un coefficient entier
        /// </summary>
        /// <param name="coefficient">
        /// Coefficient de réduction en hauteur qui sera compris entre 1 et 10 dans le WPF
        /// </param>
        public void RetrecirImageHauteur(int coefficient)
        {
            int vraiehauteur = this.hauteur / coefficient;
            if (this.hauteur % coefficient != 0)
            {
                vraiehauteur++;
            }

            Pixel[,] matricereduite = new Pixel[vraiehauteur, this.largeur];


            for (int largeur = 0; largeur < this.largeur; largeur++)
            {
                int k = 0;
                int nouvellehauteur = 0;

                while (k + coefficient - 1 < this.hauteur)
                {
                    Pixel[] tab = new Pixel[coefficient];
                    for (int i = 0; i < coefficient; i++)
                    {
                        tab[i] = this.matricedepixels[k + i, largeur];
                    }
                    matricereduite[nouvellehauteur, largeur] = Fusion(tab);




                    nouvellehauteur++;
                    k = k + coefficient;
                }


                if (nouvellehauteur == matricereduite.GetLength(0) - 1)
                {
                    Pixel[] tab = new Pixel[this.hauteur % coefficient];
                    int compteurbis = 0;
                    for (int i = this.hauteur - 1; i > this.hauteur - 1 - (this.hauteur % coefficient); i--)
                    {
                        tab[compteurbis] = this.matricedepixels[i, largeur];
                        compteurbis++;
                    }
                    matricereduite[nouvellehauteur, largeur] = Fusion(tab);
                }



            }

            this.hauteur = vraiehauteur;
            this.matricedepixels = new Pixel[this.hauteur, this.largeur];
            for (int j = 0; j < this.largeur; j++)
            {
                for (int i = 0; i < this.hauteur; i++)
                {


                    this.matricedepixels[i, j] = matricereduite[i, j];

                }
            }
        }

        /// <summary>
        /// Cette méthode permet de réduire l'image en largeur en fonction d'un coefficient entier
        /// ATTENTION : Pour qu'une image puisse être lue il faut que sa largeur soit divisible par 4, c'est pourquoi on applique une méthode de correction après
        /// </summary>
        /// <param name="coefficient">
        /// Coefficient de rédution de l'image en largeur qui sera compris entre 1 et 10 dans le WPF
        /// </param>
        public void RetrecirImageLargeur(int coefficient)
        {

            int vraielargeur = this.largeur / coefficient;

            Pixel[,] matricereduite = new Pixel[this.hauteur, vraielargeur];


            for (int hauteur = 0; hauteur < this.hauteur; hauteur++)
            {
                int k = 0;

                for (int u = 0; u < vraielargeur; u++)
                {
                    Pixel[] tab = new Pixel[coefficient];
                    for (int i = 0; i < coefficient; i++)
                    {
                        tab[i] = this.matricedepixels[hauteur, k + i];
                    }
                    matricereduite[hauteur, u] = Fusion(tab);
                    k = k + coefficient;
                }



            }

            this.largeur = vraielargeur;
            this.matricedepixels = new Pixel[this.hauteur, this.largeur];
            for (int j = 0; j < this.largeur; j++)
            {
                for (int i = 0; i < this.hauteur; i++)
                {


                    this.matricedepixels[i, j] = matricereduite[i, j];

                }
            }








        }

        /// <summary>
        /// Cette méthode permet de dissimuler une image dans une autre. Elle s'applique à l'image dans laquelle on veut cacher quelque chose
        /// </summary>
        /// <param name="imagecachée">
        /// Image que l'on veut cacher dans l'image principale
        /// </param>
        public void CoderImageDansUneAutre(MyImage imagecachée)
        {
            if (this.largeur >= imagecachée.largeur && this.hauteur >= imagecachée.hauteur)
            {
                int[] tabrougeimageprincipale = new int[this.hauteur * this.largeur];
                int[] tabbleuimageprincipale = new int[this.hauteur * this.largeur];
                int[] tabvertimageprincipale = new int[this.hauteur * this.largeur];

                int[] tabrougeimagecachée = new int[imagecachée.hauteur * imagecachée.largeur];
                int[] tabbleuimagecachée = new int[imagecachée.hauteur * imagecachée.largeur];
                int[] tabvertimagecachée = new int[imagecachée.hauteur * imagecachée.largeur];

                int[] tabrougefinal = new int[imagecachée.hauteur * imagecachée.largeur];
                int[] tabbleufinal = new int[imagecachée.hauteur * imagecachée.largeur];
                int[] tabvertfinal = new int[imagecachée.hauteur * imagecachée.largeur];

                int k = 0;
                for (int i = 0; i < imagecachée.hauteur; i++)
                {
                    for (int j = 0; j < imagecachée.largeur; j++)
                    {
                        tabrougeimageprincipale[k] = this.matricedepixels[i, j].rouge & 240; //240 = 1111 0000 en binaire --> tabrouge ne va contenir que les bits de poids forts de l'image
                        tabbleuimageprincipale[k] = this.matricedepixels[i, j].bleu & 240;
                        tabvertimageprincipale[k] = this.matricedepixels[i, j].vert & 240;

                        tabrougeimagecachée[k] = imagecachée.matricedepixels[i, j].rouge & 240;
                        tabrougeimagecachée[k] = tabrougeimagecachée[k] >> 4; //on décale ces 4 bits de poids fort vers la droite pour les avoir en tant que bits de poids faible
                        tabbleuimagecachée[k] = imagecachée.matricedepixels[i, j].bleu & 240;
                        tabbleuimagecachée[k] = tabbleuimagecachée[k] >> 4;
                        tabvertimagecachée[k] = imagecachée.matricedepixels[i, j].vert & 240;
                        tabvertimagecachée[k] = tabvertimagecachée[k] >> 4;

                        tabrougefinal[k] = tabrougeimageprincipale[k] | tabrougeimagecachée[k]; //on va mettre dans un pixel les bits de poids forts de l'image principale dans les bits de poids fort du pixel et ceux de poids forts de l'image cachée dans les bits de poids faible du pixel
                        tabbleufinal[k] = tabbleuimageprincipale[k] | tabbleuimagecachée[k];
                        tabvertfinal[k] = tabvertimageprincipale[k] | tabvertimagecachée[k];

                        k++;
                    }
                }

                k = 0;
                for (int i = 0; i < imagecachée.hauteur; i++)
                {
                    for (int j = 0; j < imagecachée.largeur; j++)
                    {
                        this.matricedepixels[i, j].rouge = tabrougefinal[k];
                        this.matricedepixels[i, j].bleu = tabbleufinal[k];
                        this.matricedepixels[i, j].vert = tabvertfinal[k];
                        k++;
                    }
                }
            }
            else
            {
                Console.WriteLine("L'image cachée possède des dimensions supérieurs à l'image principale, la méthode ne peut donc pas avoir lieu");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// On applique cette méthode sur image contenant une image cachée pour récupérer son contenu
        /// </summary>
        public void DécoderImageDansUneAutre()
        {
            int[] tabrougeimagecachée = new int[this.hauteur * this.largeur];
            int[] tabbleuimagecachée = new int[this.hauteur * this.largeur];
            int[] tabvertimagecachée = new int[this.hauteur * this.largeur];

            int k = 0;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    tabrougeimagecachée[k] = this.matricedepixels[i, j].rouge & 15; //15 = 00001111
                    tabrougeimagecachée[k] = tabrougeimagecachée[k] << 4;
                    tabbleuimagecachée[k] = this.matricedepixels[i, j].bleu & 15; //15 = 00001111
                    tabbleuimagecachée[k] = tabbleuimagecachée[k] << 4;
                    tabvertimagecachée[k] = this.matricedepixels[i, j].vert & 15; //15 = 00001111
                    tabvertimagecachée[k] = tabvertimagecachée[k] << 4;

                    tabrougeimagecachée[k] = tabrougeimagecachée[k] & 255; //255 = 11111111 --> pour remettre sur 8 bits
                    tabbleuimagecachée[k] = tabbleuimagecachée[k] & 255;
                    tabvertimagecachée[k] = tabvertimagecachée[k] & 255;

                    k++;
                }
            }

            k = 0;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    this.matricedepixels[i, j].rouge = tabrougeimagecachée[k];
                    this.matricedepixels[i, j].bleu = tabbleuimagecachée[k];
                    this.matricedepixels[i, j].vert = tabvertimagecachée[k];
                    k++;
                }
            }
        }

        /// <summary>
        /// Cette méthode permet faire de l'image actuelle un histogramme d'une image passée en paramètre
        /// </summary>
        /// <param name="image">
        /// Image dont on veut dresser l'histogramme
        /// </param>
        public void Histogramme(MyImage image)
        {
            for (int i = 0; i < image.largeur; i++)
            {
                int sommevert = 0;
                int sommerouge = 0;
                int sommebleu = 0;

                for (int j = 0; j < image.hauteur; j++)
                {
                    sommebleu = sommebleu + image.matricedepixels[j, i].bleu;
                    sommevert = sommevert + image.matricedepixels[j, i].vert;
                    sommerouge = sommerouge + image.matricedepixels[j, i].rouge;
                }

                int nbbleu = (sommebleu / 255) / 3;
                int nbvert = (sommevert / 255) / 3;
                int nbrouge = (sommerouge / 255) / 3;

                for (int k = 0; k < image.hauteur; k++)
                {

                    if (nbbleu > 0)
                    {
                        this.matricedepixels[k, i] = new Pixel(0, 0, 255);
                        nbbleu--;
                    }
                    else
                    {
                        if (nbrouge > 0)
                        {
                            this.matricedepixels[k, i] = new Pixel(255, 0, 0);
                            nbrouge--;
                        }
                        else
                        {
                            if (nbvert > 0)
                            {
                                this.matricedepixels[k, i] = new Pixel(0, 255, 0);
                                nbvert--;
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Permet de créer un Pixel qui sera la fusion de ceux d'un tableau passé en paramètre
        /// </summary>
        /// <param name="tab">
        /// Tableau de Pixel dont on va vouloir faire la fusion
        /// </param>
        /// <returns>
        /// renvoie le Pixel qui est le résultat de cette fusion
        /// </returns>
        public Pixel Fusion(Pixel[] tab)
        {

            int Red = 0;
            int Blue = 0;
            int Green = 0;

            for (int i = 0; i < tab.Length; i++)
            {
                Red = Red + tab[i].rouge;
                Blue = Blue + tab[i].bleu;
                Green = Green + tab[i].vert;
            }
            Pixel rep = new Pixel(Red / tab.Length, Green / tab.Length, Blue / tab.Length);
            return rep;
        }

        /// <summary>
        /// Permet de faire une rotation de l'image dans un sens trigonométrique, en sachant que l'origine de l'image est en bas à gauche pour nous
        /// </summary>
        public void RotationSensTrigo()
        {
            Pixel[,] matricetournée = new Pixel[this.largeur, this.hauteur];

            for (int j = 0; j < this.largeur; j++)
            {
                for (int i = 0; i < this.hauteur; i++)
                {
                    matricetournée[j, this.hauteur - i - 1] = this.matricedepixels[i, j];
                }
            }


            this.matricedepixels = null;
            this.matricedepixels = new Pixel[this.largeur, this.hauteur];
            int a = this.hauteur;
            this.hauteur = this.largeur;
            this.largeur = a;

            for (int j = 0; j < this.largeur; j++)
            {
                for (int i = 0; i < this.hauteur; i++)
                {


                    this.matricedepixels[i, j] = matricetournée[i, j];

                }
            }


        }

        /// <summary>
        /// Cette méthode permet de faire un effet miroir de l'image à laquelle on applique cette méthode par rapport à un axe vertical
        /// </summary>
        public void EffetMiroirParRapportALaVerticale()
        {
            int milieu;
            if (this.largeur % 2 == 0)
            {
                milieu = this.largeur / 2;
            }
            else
            {
                milieu = (this.largeur - 1) / 2;
            }


            for (int i = 0; i < milieu; i++)
            {
                for (int j = 0; j < this.hauteur; j++)
                {
                    Pixel Clone = new Pixel(this.matricedepixels[j, i].rouge, this.matricedepixels[j, i].vert, this.matricedepixels[j, i].bleu);
                    this.matricedepixels[j, i] = this.matricedepixels[j, this.largeur - i - 1];
                    this.matricedepixels[j, this.largeur - i - 1] = Clone;

                }
            }

        }

        /// <summary>
        /// Cette méthode permet de faire un effet miroir de l'image à laquelle on applique cette méthode par rapport à un axe horizontal
        /// </summary>
        public void EffetMiroirParRapportAHorizontale()
        {
            int milieu;
            if (this.hauteur % 2 == 0)
            {
                milieu = this.hauteur / 2;
            }
            else
            {
                milieu = (this.hauteur - 1) / 2;
            }


            for (int i = 0; i < this.largeur; i++)
            {
                for (int j = 0; j < milieu; j++)
                {
                    Pixel Clone = new Pixel(this.matricedepixels[j, i].rouge, this.matricedepixels[j, i].vert, this.matricedepixels[j, i].bleu);
                    this.matricedepixels[j, i] = this.matricedepixels[this.hauteur - j - 1, i];
                    this.matricedepixels[this.hauteur - j - 1, i] = Clone;

                }
            }

        }

        /// <summary>
        /// Cette méthode transforme l'image à laquelle on applique cette méthode en une image en noir et blanc
        /// </summary>
        public void PassageNoiretBlanc()
        {

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    this.matricedepixels[i, j].PassageNoirEtBlanc();
                }
            }
        }

        /// <summary>
        /// Applique un filtre flou à l'image
        /// </summary>
        public void FiltreFlou()
        {
            int[,] matrice = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }; // matrice 3x3 correspondant à l'effet flou

            int m = 0;
            int n = 0;


            for (int i = 1; i < this.matricedepixels.GetLength(0) - 1; i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 1; j < this.matricedepixels.GetLength(1) - 1; j++) //on ne prend pas la premiere et derniere colonne
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + this.matricedepixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + this.matricedepixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + this.matricedepixels[k, l].vert * matrice[m, n];
                            n++;
                        }
                        m++;
                        n = 0;
                    }
                    m = 0;

                    this.matricedepixels[i, j].rouge = totalrouge / 9; //--> pour le flou (divise par la somme des coeff de la matrice en paramètres)
                    this.matricedepixels[i, j].bleu = totalbleu / 9;
                    this.matricedepixels[i, j].vert = totalvert / 9;
                }
            }
        }

        /// <summary>
        /// Applique un filtre flou plus puissant à l'image
        /// </summary>
        public void FiltreFlouPlus()
        {
            int[,] matrice = new int[,] { { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 }, { 1, 1, 1, 1, 1 } };
            //matrice 5x5 correspondant à l'effet flou (plus que celui d'avant)

            int m = 0;
            int n = 0;

            //on applique le filtre sur "le centre" de l'image sans toucher au bord
            for (int i = 2; i < this.matricedepixels.GetLength(0) - 2; i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 2; j < this.matricedepixels.GetLength(1) - 2; j++) //on ne prend pas la premiere et derniere colonne
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 2; k <= i + 2; k++)
                    {
                        for (int l = j - 2; l <= j + 2; l++)
                        {
                            totalrouge = totalrouge + this.matricedepixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + this.matricedepixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + this.matricedepixels[k, l].vert * matrice[m, n];
                            n++;
                        }
                        m++;
                        n = 0;
                    }
                    m = 0;


                    this.matricedepixels[i, j].rouge = totalrouge / 25; //--> pour le flou (divise par la somme des coeff de la matrice en paramètres)
                    this.matricedepixels[i, j].bleu = totalbleu / 25;
                    this.matricedepixels[i, j].vert = totalvert / 25;
                }
            }
        }

        /// <summary>
        /// Applique un filtre de repoussage à l'image
        /// </summary>
        public void FiltreRepoussage()
        {
            int[,] matrice = new int[,] { { 0, 1, 2 }, { -1, 1, 1 }, { -2, -1, 0 } }; //matrice 3x3 correspondant à l'effet repoussage

            int m = 0;
            int n = 0;
            int[] tableaurouge = new int[this.hauteur * this.largeur];
            int[] tableaubleu = new int[this.hauteur * this.largeur];
            int[] tableauvert = new int[this.hauteur * this.largeur];

            for (int i = 0; i < this.matricedepixels.GetLength(0); i++)
            {
                tableaurouge[i] = 0;
                tableaubleu[i] = 0;
                tableauvert[i] = 0;
            }

            int g = 0;
            //on applique le filtre sur "le centre" de l'image sans toucher au bord
            for (int i = 1; i < this.matricedepixels.GetLength(0) - 1; i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 1; j < this.matricedepixels.GetLength(1) - 1; j++) //on ne prend pas la premiere et derniere colonne
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + this.matricedepixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + this.matricedepixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + this.matricedepixels[k, l].vert * matrice[m, n];
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
            for (int i = 1; i < this.matricedepixels.GetLength(0) - 1; i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 1; j < this.matricedepixels.GetLength(1) - 1; j++) //on ne prend pas la premiere et derniere colonne
                {
                    this.matricedepixels[i, j].rouge = tableaurouge[g];
                    this.matricedepixels[i, j].bleu = tableaubleu[g];
                    this.matricedepixels[i, j].vert = tableauvert[g];
                    g++;

                }
            }
        }

        /// <summary>
        /// Applique un filtre de détection de contours à l'image
        /// </summary>
        public void FiltreDetectionContours()
        {
            int[,] matrice = new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } }; //matrice 3x3 correspondant à l'effet détection contours

            int m = 0;
            int n = 0;
            int[] tableaurouge = new int[this.hauteur * this.largeur];
            int[] tableaubleu = new int[this.hauteur * this.largeur];
            int[] tableauvert = new int[this.hauteur * this.largeur];

            for (int i = 0; i < this.matricedepixels.GetLength(0); i++)
            {
                tableaurouge[i] = 0;
                tableaubleu[i] = 0;
                tableauvert[i] = 0;
            }

            int g = 0;
            //on applique le filtre sur "le centre" de l'image sans toucher au bord
            for (int i = 1; i < this.matricedepixels.GetLength(0) - 1; i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 1; j < this.matricedepixels.GetLength(1) - 1; j++) //on ne prend pas la premiere et derniere colonne
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + this.matricedepixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + this.matricedepixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + this.matricedepixels[k, l].vert * matrice[m, n];
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
            for (int i = 0; i < this.matricedepixels.GetLength(0); i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 0; j < this.matricedepixels.GetLength(1); j++) //on ne prend pas la premiere et derniere colonne
                {
                    //si on est sur les bords on met en noir car on ne peut pas appliquer le filtre sur cette partie
                    if (i == 0 || i == this.matricedepixels.GetLength(0) - 1 || j == 0 || j == this.matricedepixels.GetLength(1) - 1)
                    {
                        this.matricedepixels[i, j].rouge = 0;
                        this.matricedepixels[i, j].bleu = 0;
                        this.matricedepixels[i, j].vert = 0;
                    }
                    else
                    {
                        this.matricedepixels[i, j].rouge = tableaurouge[g];
                        this.matricedepixels[i, j].bleu = tableaubleu[g];
                        this.matricedepixels[i, j].vert = tableauvert[g];
                        g++;
                    }
                }
            }
        }

        /// <summary>
        /// Applique un filtre de renforcement des bords
        /// </summary>
        public void FiltreRenforcementDesBords()
        {
            int[,] matrice = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } }; // matrice 3x3 correspondant à l'effet renforcement des bords

            int m = 0;
            int n = 0;
            int[] tableaurouge = new int[this.hauteur * this.largeur];
            int[] tableaubleu = new int[this.hauteur * this.largeur];
            int[] tableauvert = new int[this.hauteur * this.largeur];

            for (int i = 0; i < this.matricedepixels.GetLength(0); i++)
            {
                tableaurouge[i] = 0;
                tableaubleu[i] = 0;
                tableauvert[i] = 0;
            }

            int g = 0;
            //on applique le filtre sur "le centre" de l'image sans toucher au bord
            for (int i = 1; i < this.matricedepixels.GetLength(0) - 1; i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 1; j < this.matricedepixels.GetLength(1) - 1; j++) //on ne prend pas la premiere et derniere colonne
                {
                    int totalrouge = 0;
                    int totalbleu = 0;
                    int totalvert = 0;

                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            totalrouge = totalrouge + this.matricedepixels[k, l].rouge * matrice[m, n];
                            totalbleu = totalbleu + this.matricedepixels[k, l].bleu * matrice[m, n];
                            totalvert = totalvert + this.matricedepixels[k, l].vert * matrice[m, n];
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
            for (int i = 0; i < this.matricedepixels.GetLength(0); i++) //on ne prend pas la premiere et derniere ligne
            {
                for (int j = 0; j < this.matricedepixels.GetLength(1); j++) //on ne prend pas la premiere et derniere colonne
                {
                    //si on est sur les bords on met en noir car on ne peut pas appliquer le filtre sur cette partie
                    if (i == 0 || i == this.matricedepixels.GetLength(0) - 1 || j == 0 || j == this.matricedepixels.GetLength(1) - 1)
                    {
                        this.matricedepixels[i, j].rouge = 0;
                        this.matricedepixels[i, j].bleu = 0;
                        this.matricedepixels[i, j].vert = 0;
                    }
                    else
                    {
                        this.matricedepixels[i, j].rouge = tableaurouge[g];
                        this.matricedepixels[i, j].bleu = tableaubleu[g];
                        this.matricedepixels[i, j].vert = tableauvert[g];
                        g++;
                    }
                }
            }
        }

        /// <summary>
        /// Cette méthode est très importante. Comme dit plus tôt, il faut que la largeur soit divisible par 4 pour que l'image puisse être lue.
        /// Dans le doute, on applique aussi ceci à la hauteur car nous n'avons pas trouvé d'informations claires à ce sujet.
        /// </summary>
        public void CorrectionTaillefichierdivisibilitépar4()
        {
            //on recompte la taille du fichier
            this.taillefichier = (this.hauteur * this.largeur) * 3 + 54;

            //il faut que largeur et hauteur soient divisible par 4

            int hauteurarajouter = 0;
            if (this.hauteur % 4 != 0)
            {
                hauteurarajouter = 4 - (this.hauteur % 4);
            }
            int largeurarajouter = 0;
            if (this.largeur % 4 != 0)
            {
                largeurarajouter = 4 - (this.largeur % 4);
            }


            Pixel[,] matricetemp = new Pixel[this.hauteur + hauteurarajouter, this.largeur + largeurarajouter];

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    matricetemp[i, j] = this.matricedepixels[i, j];

                }
            }

            for (int i = 0; i < this.hauteur; i++)
            {

                for (int j = 0; j < largeurarajouter; j++)
                {
                    matricetemp[i, this.largeur + j] = this.matricedepixels[i, this.largeur - 1];
                }
            }

            for (int i = 0; i < this.largeur + largeurarajouter; i++)
            {
                for (int j = 0; j < hauteurarajouter; j++)
                {
                    matricetemp[this.hauteur + j, i] = this.matricedepixels[this.hauteur - 1, i];
                }
            }


            this.matricedepixels = new Pixel[this.hauteur + hauteurarajouter, this.largeur + largeurarajouter];
            this.hauteur = this.hauteur + hauteurarajouter;
            this.largeur = this.largeur + largeurarajouter;

            for (int j = 0; j < this.largeur; j++)
            {
                for (int i = 0; i < this.hauteur; i++)
                {


                    this.matricedepixels[i, j] = matricetemp[i, j];

                }
            }

        }

        /// <summary>
        /// Cette méthode permet de transformer l'image en fractale créé avec un niveau de résolution passé en paramètre
        /// </summary>
        /// <param name="resolution">
        /// Résolution entère passée en paramètre
        /// </param>
        public void Fractale(int resolution)
        {
            double x1 = -2.1;
            double x2 = 0.6;
            double y1 = -1.2;
            double y2 = 1.2;
            double image_x = this.hauteur;
            double image_y = this.largeur;

            double zoomx = image_x / (x2 - x1);
            double zoomy = image_y / (y2 - y1);

            for (int x = 0; x < image_x; x++)
            {
                for (double y = 0; y < image_y; y++)
                {
                    //Pour tout les éléments de l'image
                    double partieréelle = (x / zoomx) + x1;
                    double partieimaginaire = (y / zoomy) + y1;

                    double termerelle = 0;
                    double termeimaginaire = 0;
                    int i = 0;

                    do
                    {
                        double temp = termerelle;
                        termerelle = (termerelle * termerelle) - (termeimaginaire * termeimaginaire) + partieréelle;
                        termeimaginaire = 2 * termeimaginaire * temp + partieimaginaire;
                        i++;
                    }
                    while (termerelle * termerelle + termeimaginaire * termeimaginaire < 4 && i < resolution);


                    if (i == resolution)
                    {
                        this.matricedepixels[Convert.ToInt32(x), Convert.ToInt32(y)].bleu = 0;
                        this.matricedepixels[Convert.ToInt32(x), Convert.ToInt32(y)].rouge = 0;
                        this.matricedepixels[Convert.ToInt32(x), Convert.ToInt32(y)].vert = 0;
                    }
                    if (i != resolution)
                    {
                        this.matricedepixels[Convert.ToInt32(x), Convert.ToInt32(y)].bleu = (i * 255) / (resolution);
                        this.matricedepixels[Convert.ToInt32(x), Convert.ToInt32(y)].rouge = 0;
                        this.matricedepixels[Convert.ToInt32(x), Convert.ToInt32(y)].vert = 0;
                    }

                }
            }
        }


        /// <summary>
        /// Cette fonction permet d'enregistrer l'image après lui avoir appliqué des correctifs.
        /// On reprend le header de l'image d'origine puis on le corrige en fonction des modifications qu'on a pu appliqué à l'image
        /// </summary>
        /// <param name="nometformat">
        /// Chemin (absolu pour le PWF et relatif dans nos premiers tests) d'enregistrement de l'image
        /// </param>
        public void EnregistrerImage(string nometformat)
        {
            byte[] tableaubytes = new byte[this.tailleoffset + this.matricedepixels.Length * 3]; //chaque pixel comporte 3 bytes, et il faut prévoir de la place pour le header et le header info
            for (int i = 0; i < 54; i++)
            {
                tableaubytes[i] = this.Header[i]; //on peut pas changer le format d'origine du coup
            }
            //on a le même header de base, maintenant on place les changements qu'on a pu faire


            int a = 0;
            //on recompte la taille du fichier
            this.taillefichier = (this.hauteur * this.largeur) * 3 + 54;

            byte[] tabtaillefichier = Convertir_Int_To_Endian(this.taillefichier, 4);
            for (int i = 0; i < 4; i++)
            {
                tableaubytes[i + 2] = tabtaillefichier[i];
            }
            byte[] tabtailleoffset = Convertir_Int_To_Endian(this.tailleoffset, 4);
            for (int i = 0; i < 4; i++)
            {
                tableaubytes[i + 10] = tabtailleoffset[i];
            }
            byte[] tablargeur = Convertir_Int_To_Endian(this.largeur, 4);
            for (int i = 0; i < 4; i++)
            {
                tableaubytes[i + 18] = tablargeur[i];
            }
            byte[] tabhauteur = Convertir_Int_To_Endian(this.hauteur, 4);
            for (int i = 0; i < 4; i++)
            {
                tableaubytes[i + 22] = tabhauteur[i];
            }
            byte[] tabbitscouleurs = Convertir_Int_To_Endian(this.nombrebitsparcouleur, 2);
            for (int i = 0; i < 2; i++)
            {
                tableaubytes[i + 28] = tabbitscouleurs[i];
            }

            int compteur = 54;
            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {
                    tableaubytes[compteur] = Convert.ToByte(this.matricedepixels[i, j].rouge);
                    tableaubytes[compteur + 1] = Convert.ToByte(this.matricedepixels[i, j].vert);
                    tableaubytes[compteur + 2] = Convert.ToByte(this.matricedepixels[i, j].bleu);
                    compteur = compteur + 3;
                }
            }




            File.WriteAllBytes(nometformat, tableaubytes);
        }

        /// <summary>
        /// Fonction de conversion de Tableau de bytes big endian à entier
        /// </summary>
        /// <param name="tab">
        /// Tableau de bytes à convertir
        /// </param>
        /// <returns>
        /// Renvoie un entier correspondant au tableau de byte
        /// </returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int reponse = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                reponse = reponse + tab[i] * Convert.ToInt32(Math.Pow(256, i));
            }

            return reponse;
        }

        /// <summary>
        /// Conversion d'entier à tableau de byte big endian
        /// </summary>
        /// <param name="valeur">
        /// Valeur à convertir en tableau byte big endian
        /// </param>
        /// <param name="taille">
        /// Taille du tableau de byte dans lequel on va convertir le nombre entier
        /// </param>
        /// <returns>
        /// Renvoie le tableau de byte correspondant à la conversion
        /// </returns>
        public byte[] Convertir_Int_To_Endian(int valeur, int taille)
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
    }
}