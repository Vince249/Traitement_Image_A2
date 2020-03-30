using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Logiciel_traitement_images
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string CheminEnregistrement = "";
        string CheminOrigine = "";


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


        /// <summary>
        /// Permet de créer une image blanche de taille libre
        /// </summary>
        /// <param name="nom">
        /// Nom que l'on va donner à l'image blanche, on va s'en servir pour l'histogramme
        /// </param>
        /// <param name="largeur">
        /// Largeur libre que l'on peut donner à l'image
        /// </param>
        /// <param name="hauteur">
        /// Hauteur libre que l'on peut donner à l'image
        /// </param>
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

        /// <summary>
        /// Cette fonction permet de récupérer le chemin de l'image
        /// </summary>
        /// <param name="indicationpourimageAchoisir">
        /// Nom de l'image par défaut, ici on ne met rien parce qu'on va en sélectionner une
        /// </param>
        /// <returns>
        /// Renvoie le chemin de l'image
        /// </returns>
        static string ParcourirFichierEtRecupererNomImage()
        {
            // Configuration de la boite de dialogue
            Microsoft.Win32.OpenFileDialog fichierimage = new Microsoft.Win32.OpenFileDialog();
            fichierimage.FileName = "";
            fichierimage.DefaultExt = ".bmp"; // extension par défaut
            fichierimage.Filter = "Image (.bmp)|*.bmp"; // 

            // Montre boite de dialogue
            Nullable<bool> result = fichierimage.ShowDialog();

            // Processus pour ouvrir l'image
            string nomfichier = null;
            if (result == true)
            {
                // Ouvrir document
                nomfichier = fichierimage.FileName;
            }
            return nomfichier;
        }

        /// <summary>
        /// Effectue le même travail que la fonction ci-dessus mais pour l'enregistement
        /// </summary>
        /// <returns>
        /// Renvoie le chemin d'enregistrement de l'image
        /// </returns>
        private string Enregistrement()
        {
            string filename = "";
            Microsoft.Win32.SaveFileDialog element = new Microsoft.Win32.SaveFileDialog();
            element.FileName = "";
            element.DefaultExt = ".bmp";
            element.Filter = "Image (.bmp)|*.bmp"; // Filtre les fichiers par leur type


            Nullable<bool> result = element.ShowDialog();


            if (result == true)
            {

                filename = element.FileName;
            }

            return filename;
        }

        /// <summary>
        /// Fonction nécessaire pour le WPF
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }


        //ON MET DES TRY/CATCH POUR EVITER QUE L'ON AIT DES ERREURS DE BUILT A CAUSE DE LA TAILLE DU MOT "nomfichier" QUI SERA NUL SI L'ON CLIQUE SUR "ANNULER"

        /// <summary>
        /// Bouton qui permet de faire passer l'image en niveau de gris en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PassageNiveauGris(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine; ;

            try
            {
                Projet_Vincent_Poupet.MyImage imagegrise = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagegrise.PassageGris();
                imagegrise.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de faire passer l'image en noir et blanc en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PassageNoirEtBlanc(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagenoireetblanc = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagenoireetblanc.PassageNoiretBlanc();
                imagenoireetblanc.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'agrandir l'image en hauteur et en largeur en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_AgrandirImage(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageagrandie = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imageagrandie.AgrandirImageHauteur(Convert.ToInt32(SliderAgrandirImage.Value));
                imageagrandie.AgrandirImageLargeur(Convert.ToInt32(SliderAgrandirImage.Value));
                imageagrandie.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'agrandir l'image en largeur en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgrandirLargeur(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageagrandie = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imageagrandie.AgrandirImageLargeur(Convert.ToInt32(SliderAgrandirLargeur.Value));

                imageagrandie.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'agrandir l'image en hauteur en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgrandirHauteur(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageagrandie = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imageagrandie.AgrandirImageHauteur(Convert.ToInt32(SliderAgrandirHauteur.Value)); //On agrandit l'image en fonction du coefficient donné par le slider

                imageagrandie.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de rétrecir l'image en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RetrecirImage(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageretrecie = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imageretrecie.RetrecirImageHauteur(Convert.ToInt32(SliderRetrecirImage.Value));
                imageretrecie.RetrecirImageLargeur(Convert.ToInt32(SliderRetrecirImage.Value));
                imageretrecie.CorrectionTaillefichierdivisibilitépar4(); //NE PAS OUBLIER CETTE FONCTION
                imageretrecie.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui peremt de rétrecir la hauteur en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetrecirHauteur(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageretrecie = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imageretrecie.RetrecirImageHauteur(Convert.ToInt32(SliderRetrecirHauteur.Value));

                imageretrecie.CorrectionTaillefichierdivisibilitépar4(); //NE PAS OUBLIER CETTE FONCTION
                imageretrecie.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de rétrecir la largeur en fonction de la valeur renvoyée par le slider associé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetrecirLargeur(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageretrecie = new Projet_Vincent_Poupet.MyImage(nomfichier);

                imageretrecie.RetrecirImageLargeur(Convert.ToInt32(SliderRetrecirLargeur.Value));
                imageretrecie.CorrectionTaillefichierdivisibilitépar4(); //NE PAS OUBLIER CETTE FONCTION
                imageretrecie.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'effectuer une rotation dans le sens trigo de 90°
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Rotation(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagerotation = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagerotation.RotationSensTrigo();
                imagerotation.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'effectuer une rotation de 90° opposé au sens trigo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RotationTriple(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagerotation = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagerotation.RotationSensTrigo();
                imagerotation.RotationSensTrigo();
                imagerotation.RotationSensTrigo();
                imagerotation.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de renverser l'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Rotation180(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagerotation = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagerotation.RotationSensTrigo();

                imagerotation.RotationSensTrigo();
                imagerotation.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'appliquer un effet miroir en fonction d'un axe vertical
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_EffetMiroirVertical(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagemiroirvertical = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagemiroirvertical.EffetMiroirParRapportALaVerticale();
                imagemiroirvertical.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'appliquer un effet miroir en fonction d'un axe horizontal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_EffetMiroirHorizontal(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;
            try
            {
                Projet_Vincent_Poupet.MyImage imagemiroirhorizontal = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagemiroirhorizontal.EffetMiroirParRapportAHorizontale();
                imagemiroirhorizontal.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'appliquer une détection de contours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DectectionContours(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;
            try
            {
                Projet_Vincent_Poupet.MyImage imagedectioncontour = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagedectioncontour.FiltreDetectionContours();
                imagedectioncontour.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'appliquer en effet de renforcement des bords
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RenforcementDesBords(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagerenforcementdesbords = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagerenforcementdesbords.FiltreRenforcementDesBords();
                imagerenforcementdesbords.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'appliquer un filtre flou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Flou(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageflou = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imageflou.FiltreFlouPlus();
                imageflou.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet d'appliquer un effet de repoussage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Repoussage(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagerepoussage = new Projet_Vincent_Poupet.MyImage(nomfichier);
                imagerepoussage.FiltreRepoussage();
                imagerepoussage.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de générer une fractale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Fractale(object sender, RoutedEventArgs e)
        {
            CreeImageBlanche("Fractale.bmp", 240, 270);
            Projet_Vincent_Poupet.MyImage Fractale = new Projet_Vincent_Poupet.MyImage("Fractale.bmp");
            Fractale.AgrandirImageLargeur(10);
            Fractale.AgrandirImageHauteur(10);

            Fractale.Fractale(40);
            Fractale.EnregistrerImage(this.CheminEnregistrement);
        }

        /// <summary>
        /// Bouton qui permet de générer l'histogramme de couleur d'une image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Histogramme(object sender, RoutedEventArgs e)
        {
            string nomfichier = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imagepourhisto = new Projet_Vincent_Poupet.MyImage(nomfichier);
                CreeImageBlanche("Histogramme.bmp", imagepourhisto.Largeur, imagepourhisto.Hauteur);
                Projet_Vincent_Poupet.MyImage Histogramme = new Projet_Vincent_Poupet.MyImage("Histogramme.bmp");
                Histogramme.Histogramme(imagepourhisto);
                Histogramme.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de cacher une image dans une autre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CoderImageDansUneAutre(object sender, RoutedEventArgs e)
        {
            string nomfichierprincipale = ParcourirFichierEtRecupererNomImage();
            string nomfichieracacher = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageprincipale = new Projet_Vincent_Poupet.MyImage(nomfichierprincipale);
                Projet_Vincent_Poupet.MyImage imageacacher = new Projet_Vincent_Poupet.MyImage(nomfichieracacher);

                imageprincipale.CoderImageDansUneAutre(imageacacher);
                imageprincipale.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de décoder une image et voir si une image est cachée dans une autre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_DécoderImageDansUneAutre(object sender, RoutedEventArgs e)
        {
            string nomfichierprincipale = this.CheminOrigine;

            try
            {
                Projet_Vincent_Poupet.MyImage imageprincipale = new Projet_Vincent_Poupet.MyImage(nomfichierprincipale);
                imageprincipale.DécoderImageDansUneAutre();
                imageprincipale.EnregistrerImage(this.CheminEnregistrement);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Bouton qui permet de sélectionner l'image à laquelle on applique les effets, filtres,...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClicOrigine(object sender, RoutedEventArgs e)
        {
            this.CheminOrigine = ParcourirFichierEtRecupererNomImage();
            try
            {
                image.Source = ((ImageSource)(System.ComponentModel.TypeDescriptor.GetConverter(typeof(ImageSource)).ConvertFromInvariantString(this.CheminOrigine)));
                Cache1.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                Cache1.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Bouton qui permet de choisir le lieu d'enregistrement de l'image modifiée ou générée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LieuEnregistrement(object sender, RoutedEventArgs e)
        {
            string chemin = Enregistrement();
            this.CheminEnregistrement = chemin;

            if (chemin != "")
            {
                Cache2.Visibility = Visibility.Hidden;
                Cache3.Visibility = Visibility.Hidden;
                Cache4.Visibility = Visibility.Hidden;
            }

        }

        /// <summary>
        /// Bouton qui permet de sélectionner une image et de l'ouvrir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OuvrirImage(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Convert.ToString(ParcourirFichierEtRecupererNomImage()));
                //on peut choisir l'image que l'on veut afficher donc peut être améliorer peut etre
            }
            catch (Exception)
            {

            }
        }


    }
}