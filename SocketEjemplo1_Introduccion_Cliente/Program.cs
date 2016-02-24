using System;
using System.Collections.Generic;
using System.Linq;

/*Using necesario para sockets*/
using System.Net.Sockets;
using System.Net;


using System.Text;
using System.Threading.Tasks;


namespace SocketEjemplo1_Introduccion_Cliente
{
    class Program
    {

        /*Configuracion del socket servidor, IPV4 y stream indica envio bidireccional*/
        private static Socket cliente = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream,
                                           ProtocolType.Tcp);


        static void Main(string[] args)
        {
            Console.Title = "Cliente";
            /*Se conecta N veces hasta que lo logra*/
            LoopConnect();
            /*Envia datos al servidor N veces*/
            SendLoop();
            /*Deja la consola permanentemente*/
            Console.ReadLine();
        }



        private static void LoopConnect()
        {
            /*Contador de intentos de conexion*/
            int intentos = 0;

            /*Mientras que no se ha conectado*/
            while (!cliente.Connected)
            {
                try
                {
                    intentos++;
                    /*Conectece al servidor*/
                    /*IPLOCAL, PUERTO DE ESCUCHA*/
                    cliente.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    /*Limpia consola y muestra mensaje*/
                    Console.Clear();
                    Console.WriteLine("Total de intentos: " + intentos);
                }
            }

            Console.Clear();
            Console.WriteLine("Usuario conectado");
        }


        private static void SendLoop()
        {
            while (true)
            {
                Console.Write("Ingrese dato a enviar:");
                /*Se lee por teclado la solicitud que se quiera mandar*/
                String req = Console.ReadLine();

                /*Se mete en un array de byte*/
                byte[] buffer = Encoding.ASCII.GetBytes(req);
                /*Se envia el dato al servidor*/
                cliente.Send(buffer);

                /*Array que almacenara respuesta del servidor*/
                byte[] receivedBuf = new byte[1024];
                /*Se saca el tamaño de los datos recibids*/
                int rec = cliente.Receive(receivedBuf);

                /*Array donde se almacenara solo el segmento de la info recibida*/
                byte[] data = new byte[rec];
                /*Se saca solo el segmento de la informacion recibida*/
                Array.Copy(receivedBuf, data, rec);

                /*Se obtiene el dato del array*/
                Console.WriteLine("Recibido: " + Encoding.ASCII.GetString(data));

            }
        }
    }
}
