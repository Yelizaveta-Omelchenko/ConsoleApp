using Smile;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                #region projekt_kredit_Region

                Network net = new Network();

                net.ReadFile("projekt_kredit(v4).xdsl");

                net.UpdateBeliefs();

               // net.get_selected_node("wydanie_pozyczki", "wydanie");
               // net.get_selected_node("wydanie_pozyczki", "odrzucenie");

                foreach (int handle in net)
                {
                    Console.WriteLine("Node {0}, {1}, {2}", handle, net[handle], net.GetNodeName(handle));
                    if (net[handle] == "wydanie_pozyczki")
                    {
                        continue;
                    }
                    net.set_evidence_selected_node(net[handle]);
                }

                net.UpdateBeliefs();

                // wydanie_pozyczki

                net.get_selected_node("wydanie_pozyczki", "wydanie");  
                net.get_selected_node("wydanie_pozyczki", "odrzucenie");

                #endregion
            }
            catch (SmileException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadKey(); //закрытие консоли после нажатия на любой баттон
        }
    }

    public static class NetworkHelper
    {
        public static int outcomeIndex;
        public static string state = "";

        public static void get_selected_node(this Network net, string node, string state)
        {
            if (!string.IsNullOrEmpty(state))
            {
                string[] aSelectedOutcomeIds = net.GetOutcomeIds(node);
                if (!aSelectedOutcomeIds.Contains(state))
                {
                    throw new Exception($"state {state} in {node} not found");
                }
                for (outcomeIndex = 0; outcomeIndex < aSelectedOutcomeIds.Length; outcomeIndex++)
                    if (state.Equals(aSelectedOutcomeIds[outcomeIndex]))
                        break;

                double[] aValues = net.GetNodeValue(node);
                double P_Selected_Node_Value = aValues[outcomeIndex];

                Console.WriteLine($"P({node} = {state}) = {P_Selected_Node_Value}");
            }
            else
            {
                throw new Exception($"uncorect value for the state in {node}");
            }
        }

        public static void set_selected_node(this Network net, string node, string state)
        {
            net.get_selected_node(node, state);

            net.SetEvidence(node, state);
        }

        public static void set_evidence_selected_node(this Network net, string node)
        {
            if (!string.IsNullOrEmpty(node))
            {
                string[] aOutcomeIds = net.GetOutcomeIds(node); //get current state in this node
                Console.WriteLine($"Set {node} ({aOutcomeIds.ConcatString()}) :"); //
                state = Console.ReadLine();
                if (!aOutcomeIds.Contains(state))
                {
                    throw new Exception($"state {state} in {node} not found");
                }

                net.SetEvidence(node, state);
            }
            else
            {
                throw new Exception($"uncorect value for the {node}");
            }
        }

        public static string ConcatString(this string[] str)
        {
            string concatStr = "";
            foreach (string single in str)
            {
                concatStr += " " + single;
            }
            return concatStr;
        }
    }

}
