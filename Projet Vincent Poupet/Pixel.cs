using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Vincent_Poupet
{

    /// <summary>
    /// Classe Pixel qui permet d'éviter d'utiliser uniquement des tableaux de bytes
    /// </summary>
    public class Pixel
    {
        //on crée cette classe pour pouvoir créer une matrice de Pixels par la suite
        private int RED;
        private int GREEN;
        private int BLUE;

        /// <summary>
        /// Constructeur de la classe Pixel
        /// </summary>
        /// <param name="RED">
        /// Valeur du byte RED du Pixel
        /// </param>
        /// <param name="GREEN">
        /// Valeur du byte GREEN du Pixel
        /// </param>
        /// <param name="BLUE">
        /// Valeur dy byte BLUE du Pixel
        /// </param>
        public Pixel(int RED, int GREEN, int BLUE)
        {
            if (RED < 256 && GREEN < 256 && BLUE < 256 && RED >= 0 && GREEN >= 0 && BLUE >= 0)
            {
                this.RED = RED;
                this.GREEN = GREEN;
                this.BLUE = BLUE;
            }

        }

        /// <summary>
        /// Permet de passer le Pixel en niveau de gris
        /// </summary>
        public void PassageGris()
        {
            int total = this.RED + this.GREEN + this.BLUE;
            this.RED = total / 3;
            this.GREEN = this.RED;
            this.BLUE = this.RED;
        }

        /// <summary>
        /// Permet de passer le Pixel en noir et blanc
        /// </summary>
        public void PassageNoirEtBlanc()
        {
            if (this.RED + this.BLUE + this.GREEN > ((255 * 3) / 2))
            {
                this.RED = 255;
                this.BLUE = 255;
                this.GREEN = 255;
            }
            else
            {
                this.RED = 0;
                this.BLUE = 0;
                this.GREEN = 0;
            }
        }

        /// <summary>
        /// Fonction que nous avons conservé malgré le fait qu'elle ne serve à rien car nous apprécions son résultat
        /// </summary>
        public void PassageBizarre()
        {

            if (this.RED > 127)
            {
                this.RED = 255;
            }
            else
            {
                this.RED = 0;
            }

            if (this.BLUE > 127)
            {
                this.BLUE = 255;
            }
            else
            {
                this.BLUE = 0;
            }

            if (this.GREEN > 127)
            {
                this.GREEN = 255;
            }
            else
            {
                this.GREEN = 0;
            }
        }

        /// <summary>
        /// Fonction qui permet de récupérer la valeur ROUGE du Pixel et la fixer
        /// </summary>
        public int rouge
        {
            get
            {
                return this.RED;
            }
            set
            {
                this.RED = value;
            }
        }

        /// <summary>
        /// Fonction qui permet de récupérer la valeur BLEU du Pixel et la fixer
        /// </summary>
        public int bleu
        {
            get
            {
                return this.BLUE;
            }

            set
            {
                this.BLUE = value;
            }
        }

        /// <summary>
        /// Fonction qui permet de récupérer la valeur VERT du Pixel et la fixer
        /// </summary>
        public int vert
        {
            get
            {
                return this.GREEN;
            }

            set
            {
                this.GREEN = value;
            }
        }
    }
}