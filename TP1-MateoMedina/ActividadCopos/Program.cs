using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActividadCopos
{
    /*
     El programa debe cumplir con las siguientes condiciones:
        Definir una clase Configuracion que almacene parámetros de la simulación, como la cantidad de filas, columnas y la velocidad de caída de los copos.
        Definir una clase Copo que modele el comportamiento de un copo de nieve. Cada copo debe tener una posición en la consola y un método para mostrarse y desplazarse hacia abajo.
        Usar una lista para administrar todos los copos activos durante la simulación.
        Implementar una lógica que controle la caída de los copos de nieve, evitando que se superpongan en la misma posición.
        Al completarse una fila con copos en todas las columnas, esta debe eliminarse para permitir que continúe la simulación.
        El programa debe ejecutarse en un ciclo continuo, simulando de manera animada la caída de los copos.
     
     */

    class Copos
    {
        int altura;
        int ancho;


        public int Altura
        {
            get { return altura; }
            set { altura = value; }
        }
    
        public int Ancho
        {
            get { return ancho; }
            set { ancho = value; }
        }

        public  Copos(int altura, int ancho)
        {
            this.altura = altura;
            this.ancho = ancho;
        }

        public bool mover(string[,] tabla, int limite)
        {
            int nuevaAltura = altura + 1; 

            if (nuevaAltura >= limite)
            {
                return false;
            }

            if (tabla[nuevaAltura, ancho] == "*")
            {
                return false;
            }

            altura = nuevaAltura;
            return true;

        }

    }


    class configuracion
    {
        int filas;
        int columnas;
        int velocidad;
        public int Filas
        {
            get { return filas; }
            set { filas = value; }
        }
        public int Columnas
        {
            get { return columnas; }
            set { columnas = value; }
        }
        public int Velocidad
        {
            get { return velocidad; }
            set { velocidad = value; }
        }
        public configuracion(int filas, int columnas, int velocidad)
        {
            this.filas = filas;
            this.columnas = columnas;
            this.velocidad = velocidad;
        }

    }

    class programa
    {
        static void Main(string[] args)
        {
            configuracion config = new configuracion(10,10,200);
            //la velocidad esta en milisegundos

            string[,] tabla = new string[config.Filas, config.Columnas];


            Console.CursorVisible = false;
            Console.SetWindowSize(config.Columnas, config.Filas);

            List<Copos> coposActivos = new List<Copos>();
            Random valorRandom = new Random();


            //esto va a hacer que se haga continuamente
            while (true) {

                //esto crea el array entero, como seria la tabla de la consola, y lo llena de espacios vacios
                for (int i = 0; i < config.Filas; i++)
            {
                for (int j = 0; j < config.Columnas; j++)
                {
                    tabla[i, j] = " ";
                }
            }

                
                //consigo un valor random para ponerle a la columna random
                int columnaRandom = valorRandom.Next(0, config.Columnas);


                //esto verifica si hay algun copo activo en la misma columna y altura 0, si no hay, crea un nuevo copo en esa posicion, si hay, no hace nada
               if (!coposActivos.Any(copo => copo.Ancho == columnaRandom && copo.Altura == 0))
                {

                        Copos copo = new Copos(0, columnaRandom);
                        coposActivos.Add(copo);
                }
                

                //esto es para ordenar los copos activos de mayor a menor altura, para que se dibujen primero los que estan mas abajo y no se sobrepongan
                var coposOrdenados = coposActivos.OrderByDescending(copo => copo.Altura).ToList();
                //var es para que el programa busque de que tipo es la variable

                //aca escribo el copo en la posicion
                foreach (Copos copo in coposOrdenados)
                {
                    copo.mover(tabla, config.Filas);
                    tabla[copo.Altura, copo.Ancho] = "*";
                }

                //aca busco en todas las filas, y pongo el -1 porque son 10 filas pero el array cuenta hasta 9 porque arranca en 0
                for (int i = config.Filas - 1; i >= 0; i--)
                {
                    bool filaLLenada = true;
                    //la fila esta llena de default

                    for (int c = 0; c < config.Columnas; c++)
                    {
                        //esto hace que en caso de que no este llena la fila, pase a buscar la otra 
                        if (tabla[i, c] != "*")
                        {
                            filaLLenada = false;
                            break;
                        }

                    }

                    //aca borro la fila llena y bajo todos los copos que estan arriba de esa fila
                    if (filaLLenada)
                    {
                        int filaAEliminar = i;

                        coposActivos.RemoveAll(copo => copo.Altura == filaAEliminar);

                        foreach (Copos copo in coposActivos)
                        {
                            if (copo.Altura < filaAEliminar)
                            {
                                copo.Altura++;
                                //esto le agrega 1 a altura, osea, lo baja 1 puesto
                            }

                        }


                        //aca creo la tabla de vuelta, y le escribo todos los copos en la posicion que les corresponde, para que se dibujen en la consola
                        tabla = new string[config.Filas, config.Columnas];

                        for (int D = 0; D < config.Filas; D++)
                        {
                            for (int j = 0; j < config.Columnas; j++)
                            {
                                tabla[D, j] = " ";
                            }
                        }

                        foreach (Copos copo in coposActivos)
                        {
                            tabla[copo.Altura, copo.Ancho] = "*";
                        }
                        i++;
                    }

                    
                }
                Console.SetCursorPosition(0, 0);

                for (int i = 0; i < config.Filas; i++)
                {
                    for (int j = 0; j < config.Columnas; j++)
                    {
                        Console.Write(tabla[i, j]);
                    }
                    Console.WriteLine();
                }

                //el thread es una funcion para dejar descansando el programa, entonces va a como frenarlo, y despues lo deja seguir 
                Thread.Sleep(config.Velocidad);
            }
        }
    }
}
