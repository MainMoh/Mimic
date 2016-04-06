using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sulakore;
using Sulakore.Communication;
using Sulakore.Habbo.Web;
using Sulakore.Protocol;
using Sulakore.Habbo;
using Sulakore.Components;
using System.IO;
using System.Diagnostics;
using Tangine;
using Sulakore.Modules;
using Tangine.Habbo;
using System.Globalization;

namespace Mohs_Mimic_Tool
{
    [Module("Moh's Mimic Tool", "Mimic them to death!")]
    [Author("Moh", ResourceName = "", ResourceUrl = "", HabboName = "", Hotel = HHotel.Nl)]
    [GitHub("", "")]

    public partial class Form1 : ExtensionForm
    {
        #region Others
        public string UserName = string.Empty;
        public int UserIndex = -1;
        #endregion

        #region Booleans
        public bool BotOn = false;
        public bool MimicClothes = true;
        public bool MimicActions = true;
        public bool MimicMotto = true;
        public bool MimicSpeech = true;
        public bool MimicShout = true;
        public bool MimicWhisper = true;
        public bool MimicDance = true;
        public bool MimicSigns = true;
        public bool UseMimicX = false;
        #endregion

        #region Client Side Headers
        public ushort csClothes = 0;
        public ushort csMotto = 0;
        public ushort csAction = 0;
        public ushort csIncomingUser = 0;
        public ushort csDance = 0;
        public ushort csSpeech = 0;
        public ushort csShout = 0;
        public ushort csSign = 0;
        public ushort csWhisper = 0;
        #endregion

        #region Server Side Headers
        public ushort ssClothes = 0;
        public ushort ssMotto = 0;
        public ushort ssAction = 0;
        public ushort ssEnterRoom = 0;
        public ushort ssDance = 0;
        public ushort ssSpeech = 0;
        public ushort ssShout = 0;
        public ushort ssSign = 0;
        public ushort ssWhisper = 0;
        #endregion

        #region Events

        public void GetHeaders()
        {
            var TempSSShout = Game.GetMessages("4c310dd04b5f13450d02d224ba31580d")[0];
            var TempSSSpeech = Game.GetMessages("0112c940712e9061bdaa2b9581216551")[0];
            var TempSSWhisper = Game.GetMessages("e1845dadbaa2b01ece59eb127bb6cecc")[0];
            var TempSSClothes = Game.GetMessages("9664386bf6a5ba35b3d60c6cce75b962")[0];
            var TempSSMotto = Game.GetMessages("2e1f81e3c5411c277baddc66e052281e")[0];
            var TempSSDance = Game.GetMessages("823aaa0e550fcf191979840af4af8506")[0];
            var TempSSAction = Game.GetMessages("5ba7e2b2897b1cca6988574139469a1b")[0];
            var TempSSSign = Game.GetMessages("550aaa1f7fa506b36041fe88c0ba45b7")[0];
            var TempSSEnterRoom = Game.GetMessages("3602fe9addff37ad6a950e136aa87822")[0];
            ssShout = Game.GetMessageHeader(TempSSShout);
            ssSpeech = Game.GetMessageHeader(TempSSSpeech);
            ssDance = Game.GetMessageHeader(TempSSDance);
            ssWhisper = Game.GetMessageHeader(TempSSWhisper);
            ssAction = Game.GetMessageHeader(TempSSAction);
            ssEnterRoom = Game.GetMessageHeader(TempSSEnterRoom);
            ssSign = Game.GetMessageHeader(TempSSSign);
            ssClothes = Game.GetMessageHeader(TempSSClothes);
            ssMotto = Game.GetMessageHeader(TempSSMotto);

            var TempCSShout = Game.GetMessages("7f7161f371aee05b169eb5b0d5edf48b")[0];
            var TempCSSpeech = Game.GetMessages("4dd6890e7a13d7c88657e00d14c2ac52")[0];
            var TempCSWhisper = Game.GetMessages("e242c1253a4a02e4992601fd900f7be0")[0];
            var TempCSClothes = Game.GetMessages("f153aa5cb130f529d796a11303419470")[0];
            var TempCSMotto = Game.GetMessages("f153aa5cb130f529d796a11303419470")[0];
            var TempCSDance = Game.GetMessages("52293774e5cabc4ad4ea74ff1339080c")[0];
            var TempCSAction = Game.GetMessages("64bac1e39743a9faa2084006f5071a37")[0];
            var TempCSSign = Game.GetMessages("45d53173f4bf410c6f0d57f0fb0edca3")[0];
            var TempCSIncomingUser = Game.GetMessages("4edb850e832ac9873c9650d68779fc4d")[0];
            csShout = Game.GetMessageHeader(TempCSShout);
            csSpeech = Game.GetMessageHeader(TempCSSpeech);
            csDance = Game.GetMessageHeader(TempCSDance);
            csWhisper = Game.GetMessageHeader(TempCSWhisper);
            csAction = Game.GetMessageHeader(TempCSAction);
            csIncomingUser = Game.GetMessageHeader(TempCSIncomingUser);
            csSign = Game.GetMessageHeader(TempCSSign);
            csClothes = Game.GetMessageHeader(TempCSClothes);
            csMotto = Game.GetMessageHeader(TempCSMotto);
        }

        public Form1()
        {
            InitializeComponent();
            GetHeaders();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        private async void playerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string d = playerList.SelectedItem.ToString();
            string[] r = d.Split('`');
            UserIndex = int.Parse(r[0]);
            UserName = r[1];
            string Look = r[2];
            string Gender;
            if (r[3].ToLower() == "female")
            {
                Gender = "F";
            }
            else
            {
                Gender = "M";
            }
            string Motto = r[4];

            if (MimicClothes)
            {
                await Connection.SendToServerAsync(ssClothes, Gender, Look);
            }

            if (MimicMotto)
            {
                await Connection.SendToServerAsync(ssMotto, Motto);
            }
        }

        public static string GetStringInBetween(string strBegin, string strEnd, string strSource, bool includeBegin, bool includeEnd)
        {
            string[] result = { string.Empty, string.Empty };
            int iIndexOfBegin = strSource.IndexOf(strBegin);

            if (iIndexOfBegin != -1)
            {
                // include the Begin string if desired 
                if (includeBegin)
                    iIndexOfBegin -= strBegin.Length;

                strSource = strSource.Substring(iIndexOfBegin + strBegin.Length);

                int iEnd = strSource.IndexOf(strEnd);
                if (iEnd != -1)
                {
                    // include the End string if desired 
                    if (includeEnd)
                        iEnd += strEnd.Length;
                    result[0] = strSource.Substring(0, iEnd);
                    // advance beyond this segment 
                    if (iEnd + strEnd.Length < strSource.Length)
                        result[1] = strSource.Substring(iEnd + strEnd.Length);
                }
            }
            else
                // stay where we are 
                result[1] = strSource;
            return result[0];
        }
        #endregion

        #region Packet Conditions & Movement

        private async void IncomingAction(DataInterceptedEventArgs e)
        {
            int PlayerIndex = e.Packet.ReadInteger();
            int Type = e.Packet.ReadInteger();
            if (PlayerIndex == UserIndex && MimicActions)
            {
                await Connection.SendToServerAsync(ssAction, Type);
            }
        }

        private async void IncomingClothes(DataInterceptedEventArgs e)
        {
            int PlayerIndex = e.Packet.ReadInteger();
            string Look = e.Packet.ReadString();
            string Gender = e.Packet.ReadString();
            string Motto = e.Packet.ReadString();
            if (PlayerIndex == UserIndex && MimicClothes)
            {
                await Connection.SendToServerAsync(ssClothes, Gender, Look);
            }
        }

        private async void IncomingDance(DataInterceptedEventArgs e)
        {
            int PlayerIndex = e.Packet.ReadInteger();
            int Type = e.Packet.ReadInteger();
            if (PlayerIndex == UserIndex && MimicDance)
            {
                await Connection.SendToServerAsync(ssDance, Type);
            }
        }

        private void IncomingUser(DataInterceptedEventArgs e)
        {
            int entityCount = e.Packet.ReadInteger();
            var entityList = new List<HEntity>(entityCount);

            for (int i = 0; i < entityList.Capacity; i++)
            {
                int id = e.Packet.ReadInteger();
                string name = e.Packet.ReadString();
                string motto = e.Packet.ReadString();
                string figureId = e.Packet.ReadString();
                int index = e.Packet.ReadInteger();
                int x = e.Packet.ReadInteger();
                int y = e.Packet.ReadInteger();

                var z = double.Parse(
                    e.Packet.ReadString(), CultureInfo.InvariantCulture);

                e.Packet.ReadInteger();
                int type = e.Packet.ReadInteger();

                HGender gender = HGender.Unisex;
                string favoriteGroup = string.Empty;
                #region Switch: type
                switch (type)
                {
                    case 1:
                        {
                            gender = SKore.ToGender(e.Packet.ReadString());
                            e.Packet.ReadInteger();
                            e.Packet.ReadInteger();
                            favoriteGroup = e.Packet.ReadString();
                            e.Packet.ReadString();
                            e.Packet.ReadInteger();
                            e.Packet.ReadBoolean();

                            break;
                        }
                    case 2:
                        {
                            e.Packet.ReadInteger();
                            e.Packet.ReadInteger();
                            e.Packet.ReadString();
                            e.Packet.ReadInteger();
                            e.Packet.ReadBoolean();
                            e.Packet.ReadBoolean();
                            e.Packet.ReadBoolean();
                            e.Packet.ReadBoolean();
                            e.Packet.ReadBoolean();
                            e.Packet.ReadBoolean();
                            e.Packet.ReadInteger();
                            e.Packet.ReadString();
                            break;
                        }
                    case 4:
                        {
                            e.Packet.ReadString();
                            e.Packet.ReadInteger();
                            e.Packet.ReadString();

                            for (int j = e.Packet.ReadInteger(); j > 0; j--)
                                e.Packet.ReadShort();

                            break;
                        }
                }
                #endregion

                var entity = new HEntity(id, index, name,
                    new HPoint(x, y, z), motto, gender, figureId, favoriteGroup);

                entityList.Add(entity);
            }

            foreach (HEntity Player in entityList)
            {
                playerList.Items.Add(Player.Index + "`" + Player.Name + "`" + Player.FigureId + "`" + Player.Gender + "`" + Player.Motto);
            }

        }

        private async void IncomingMotto(DataInterceptedEventArgs e)
        {
            int PlayerIndex = e.Packet.ReadInteger();
            string Look = e.Packet.ReadString();
            string Gender = e.Packet.ReadString();
            string Motto = e.Packet.ReadString();
            if (PlayerIndex == UserIndex && MimicMotto)
            {
                await Connection.SendToServerAsync(ssMotto, Motto);
            }
        }

        private async void IncomingShout(DataInterceptedEventArgs e)
        {
            int PlayerIndex = e.Packet.ReadInteger();
            string Text =   e.Packet.ReadString();
            int z = e.Packet.ReadInteger();
            int BubbleId = e.Packet.ReadInteger();
            if (PlayerIndex == UserIndex && MimicShout)
            {
                await Connection.SendToServerAsync(ssShout, Text, BubbleId);
            }
        }

        private async void IncomingSign(DataInterceptedEventArgs e)
        {
            int idk = e.Packet.ReadInteger();
            int Playerindex = e.Packet.ReadInteger();
            int idk2 = e.Packet.ReadInteger();
            int idk3 = e.Packet.ReadInteger();
            string idk4 = e.Packet.ReadString();
            int idk5 = e.Packet.ReadInteger();
            int idk6 = e.Packet.ReadInteger();
            string idk7 = e.Packet.ReadString();
            if (Playerindex == UserIndex && MimicSigns && idk7.Contains("sign"))
            {
                string InBetween = GetStringInBetween("//sign ", "/", idk7, false, false);
                await Connection.SendToServerAsync(ssSign, int.Parse(InBetween));
            }
        }

        private async void IncomingSpeech(DataInterceptedEventArgs e)
        {
            int Playerindex = e.Packet.ReadInteger();
            string Text = e.Packet.ReadString();
            int Pz = e.Packet.ReadInteger();
            int BubbleID = e.Packet.ReadInteger();
            if (Playerindex == UserIndex && MimicSpeech)
            {
                await Connection.SendToServerAsync(ssSpeech, Text, BubbleID, 1);
            }
        }

        private async void IncomingWhisper(DataInterceptedEventArgs e)
        {
            int r = e.Packet.ReadInteger();
            string Text = e.Packet.ReadString();
            int PlayerIndex = e.Packet.ReadInteger();
            int bubbleId = e.Packet.ReadInteger();
            if (PlayerIndex == UserIndex && MimicWhisper)
            {
                string tosend = UserName + " " + Text;
                await Connection.SendToServerAsync(ssWhisper, tosend, bubbleId);
            }
        }

        private void OutgoingRoom(DataInterceptedEventArgs e)
        {
            playerList.Items.Clear();
            UserName = String.Empty;
            UserIndex = -1;
        }

        private async void OutgoingSpeech(DataInterceptedEventArgs e)
        {
            string er = e.Packet.ReadString();
            if (UseMimicX && er.ToLower().Contains(":mimic "))
            {
                e.IsBlocked = true;
                string[] bit = er.Split(' ');
                string name = bit[1];
                for (int i = 0; i < playerList.Items.Count; i++)
                {
                    string comboboxStr = playerList.Items[i].ToString();
                    if (comboboxStr.Contains("`" + name + "`"))
                    {
                        string d = comboboxStr;
                        string[] r = d.Split('`');
                        UserIndex = int.Parse(r[0]);
                        UserName = r[1];
                        string Look = r[2];
                        string Gender;
                        if (r[3].ToLower() == "female")
                        {
                            Gender = "F";
                        }
                        else
                        {
                            Gender = "M";
                        }
                        string Motto = r[4];

                        if (MimicClothes)
                        {
                            await Connection.SendToServerAsync(ssClothes, Gender, Look);
                        }

                        if (MimicMotto)
                        {
                            await Connection.SendToServerAsync(ssMotto, Motto);
                        }
                        i = playerList.Items.Count;
                    }
                }
            }
        }

        private async void OutgoingShout(DataInterceptedEventArgs e)
        {
            string er = e.Packet.ReadString();
            if (UseMimicX && er.ToLower().Contains(":mimic "))
            {
                e.IsBlocked = true;
                string[] bit = er.Split(' ');
                string name = bit[1];
                for (int i = 0; i < playerList.Items.Count; i++)
                {
                    string comboboxStr = playerList.Items[i].ToString();
                    if (comboboxStr.Contains("`" + name + "`"))
                    {
                        string d = comboboxStr;
                        string[] r = d.Split('`');
                        UserIndex = int.Parse(r[0]);
                        UserName = r[1];
                        string Look = r[2];
                        string Gender;
                        if (r[3].ToLower() == "female")
                        {
                            Gender = "F";
                        }
                        else
                        {
                            Gender = "M";
                        }
                        string Motto = r[4];

                        if (MimicClothes)
                        {
                            await Connection.SendToServerAsync(ssClothes, Gender, Look);
                        }

                        if (MimicMotto)
                        {
                            await Connection.SendToServerAsync(ssMotto, Motto);
                        }
                        i = playerList.Items.Count;
                    }
                }
            }
        }
        #endregion

        #region Toggles
        private void monoFlat_Toggle1_ToggledChanged()
        {
            if (monoFlat_Toggle1.Toggled)
            {
               // Triggers.EntityLoad += IncomingUser;
                Triggers.InAttach(csIncomingUser, IncomingUser);
                Triggers.InAttach(csAction, IncomingAction);
                Triggers.InAttach(csClothes, IncomingClothes);
                Triggers.InAttach(csDance, IncomingDance);
                Triggers.InAttach(csMotto, IncomingMotto);
                Triggers.InAttach(csShout, IncomingShout);
                Triggers.InAttach(csSign, IncomingSign);
                Triggers.InAttach(csSpeech, IncomingSpeech);
                Triggers.InAttach(csWhisper, IncomingWhisper);
                Triggers.OutAttach(ssEnterRoom, OutgoingRoom);
                Triggers.OutAttach(ssSpeech, OutgoingSpeech);
                Triggers.OutAttach(ssShout, OutgoingShout);
            }
            else
            {
                Triggers.InDetach(csIncomingUser);
                Triggers.InDetach(csAction);
                Triggers.InDetach(csClothes);
                Triggers.InDetach(csDance);
                Triggers.InDetach(csIncomingUser);
                Triggers.InDetach(csMotto);
                Triggers.InDetach(csShout);
                Triggers.InDetach(csSign);
                Triggers.InDetach(csSpeech);
                Triggers.InDetach(csWhisper);
                Triggers.OutDetach(ssEnterRoom);
                Triggers.OutDetach(ssSpeech);
                Triggers.OutDetach(ssShout);
            }
        }

        private void monoFlat_Toggle3_ToggledChanged()
        {
            if (monoFlat_Toggle3.Toggled)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        private void monoFlat_Toggle2_ToggledChanged()
        {
            if (monoFlat_Toggle2.Toggled)
            {
                MimicClothes = true;
            }
            else
            {
                MimicClothes = false;
            }
        }

        private void monoFlat_Toggle4_ToggledChanged()
        {
            if (monoFlat_Toggle4.Toggled)
            {
                MimicMotto = true;
            }
            else
            {
                MimicMotto = false;
            }
        }

        private void monoFlat_Toggle5_ToggledChanged()
        {
            if (monoFlat_Toggle5.Toggled)
            {
                UseMimicX = true;
            }
            else
            {
                UseMimicX = false;
            }
        }

        private void monoFlat_Toggle7_ToggledChanged()
        {
            if (monoFlat_Toggle7.Toggled)
            {
                MimicActions = true;
            }
            else
            {
                MimicActions = false;
            }
        }

        private void monoFlat_Toggle6_ToggledChanged()
        {
            if (monoFlat_Toggle6.Toggled)
            {
                MimicSpeech = true;
            }
            else
            {
                MimicSpeech = false;
            }
        }

        private void monoFlat_Toggle11_ToggledChanged()
        {
            if (monoFlat_Toggle11.Toggled)
            {
                MimicShout = true;
            }
            else
            {
                MimicShout = false;
            }
        }

        private void monoFlat_Toggle10_ToggledChanged()
        {
            if (monoFlat_Toggle10.Toggled)
            {
                MimicWhisper = true;
            }
            else
            {
                MimicWhisper = false;
            }
        }

        private void monoFlat_Toggle9_ToggledChanged()
        {
            if (monoFlat_Toggle9.Toggled)
            {
                MimicDance = true;
            }
            else
            {
                MimicDance = false;
            }
        }

        private void monoFlat_Toggle8_ToggledChanged()
        {
            if (monoFlat_Toggle8.Toggled)
            {
                MimicSigns = true;
            }
            else
            {
                MimicSigns = false;
            }
        }
        #endregion
    }
}
