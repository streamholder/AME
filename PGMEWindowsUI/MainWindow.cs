// Awesome Map Editor
// A map editor for GBA Pokémon games.

// Copyright (C) 2015-2016 Diegoisawesome

// This file is part of Awesome Map Editor.
// Awesome Map Editor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Awesome Map Editor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Awesome Map Editor. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PGMEBackend;
using static PGMEBackend.Config;
using System.Resources;
using System.Text.RegularExpressions;
using Be.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.IO;
using PGMEBackend.Entities;
using PGMEBackend.GLControls;
using System.Reflection;

namespace PGMEWindowsUI
{
    public partial class MainWindow : Form, UIInteractionLayer
    {
        bool editorTabLoaded = false;
        static ImageList _imageListMapTree;
        public static ImageList imageListMapTree
        {
            get
            {
                if (_imageListMapTree == null)
                {
                    _imageListMapTree = new ImageList();
                    _imageListMapTree.Images.Add("Map", Properties.Resources.map_16x16);
                    _imageListMapTree.Images.Add("Map Selected", Properties.Resources.image_16x16);
                    _imageListMapTree.Images.Add("Map Folder Closed", Properties.Resources.folder_closed_map_16x16);
                    _imageListMapTree.Images.Add("Map Folder Open", Properties.Resources.folder_map_16x16);
                    _imageListMapTree.Images.Add("Folder Closed", Properties.Resources.folder_closed_16x16);
                    _imageListMapTree.ColorDepth = ColorDepth.Depth32Bit;
                }
                return _imageListMapTree;
            }
        }

        static ImageList _imageListTabControl;
        public static ImageList imageListTabControl
        {
            get
            {
                if (_imageListTabControl == null)
                {
                    _imageListTabControl = new ImageList();
                    _imageListTabControl.Images.Add(Properties.Resources.map_16x16);
                    _imageListTabControl.Images.Add(Properties.Resources.viewsprites_16x16);
                    _imageListTabControl.Images.Add(Properties.Resources.wildgrass_16x16);
                    _imageListTabControl.Images.Add(Properties.Resources.map_header_16x16);
                    _imageListTabControl.ColorDepth = ColorDepth.Depth32Bit;
                }
                return _imageListTabControl;
            }
        }

        public Dictionary<int, TreeNode> mapTreeNodes;
        TreeNode currentTreeNode;

        public MainWindow()
        {
            PGMEBackend.Program.Initialize(this);
            if (ReadConfig() != 0)
            {
                QuitApplication(0);
            }

            if (!string.IsNullOrEmpty(settings.Language))
            {
                // Sets the culture
                Thread.CurrentThread.CurrentCulture = new CultureInfo(settings.Language);
                // Sets the UI culture
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(settings.Language);
            }
            else
            {
                settings.Language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                WriteConfig();
            }

            InitializeComponent();
            LoadConfig();

            mapListTreeView.ImageList = imageListMapTree;
            mainTabControl.ImageList = imageListTabControl;
            for (int i = 0; i < mainTabControl.TabPages.Count; i++)
                mainTabControl.TabPages[i].ImageIndex = i;
            PGMEBackend.Program.SetMainGUITitle(this.Text);
            SetMapSortOrder(settings.MapSortOrder);
            mapTreeNodes = new Dictionary<int, TreeNode>();
        }

        private void LoadConfig()
        {
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        public void QuitApplication(int code)
        {
            if (Application.MessageLoop)
            {
                Application.Exit();
            }
            else
            {
                Environment.Exit(code);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PGMEBackend.Program.LoadROM();
        }

        private void toolStripMenuItemOpenROM_Click(object sender, EventArgs e)
        {
            PGMEBackend.Program.LoadROM();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void blockEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void mapAndBlocksSplitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        static Dictionary<string, MessageBoxButtons> BoxButtons = new Dictionary<string, MessageBoxButtons>
            {
                { "OK", MessageBoxButtons.OK },
                { "OKCancel", MessageBoxButtons.OKCancel },
                { "RetryCancel", MessageBoxButtons.RetryCancel },
                { "YesNo", MessageBoxButtons.YesNo },
                { "YesNoCancel", MessageBoxButtons.YesNoCancel }
            };

        static Dictionary<string, MessageBoxIcon> BoxIcons = new Dictionary<string, MessageBoxIcon>
            {
                { "Error", MessageBoxIcon.Error },
                { "Information", MessageBoxIcon.Information },
                { "Warning", MessageBoxIcon.Warning },
                { "None", MessageBoxIcon.None }
            };

        static Dictionary<DialogResult, string> DialogResults = new Dictionary<DialogResult, string>
            {
                { DialogResult.Cancel, "Cancel" },
                { DialogResult.No, "No" },
                { DialogResult.None, "None" },
                { DialogResult.OK, "OK" },
                { DialogResult.Yes, "Yes" },
            };

        public string ShowMessageBox(string body, string title)
        {
            return ShowMessageBox(body, title, "OK", "None");
        }

        public string ShowMessageBox(string body, string title, string buttons)
        {
            return ShowMessageBox(body, title, buttons, "None");
        }

        public string ShowMessageBox(string body, string title, string buttons, string icon)
        {
            return DialogResults[MessageBox.Show(body, title, BoxButtons[buttons], BoxIcons[icon])];
        }
        
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public void EnableControlsOnROMLoad()
        {
            //Enable main toolstrip buttons
            toolStripBlockEditor.Enabled = true;
            toolStripConnectionEditor.Enabled = true;
            toolStripWorldMapEditor.Enabled = true;
            toolStripPluginManager.Enabled = true;
            toolStripButton9.Enabled = true;

            //Enable map list tree
            mapListTreeView.Enabled = true;
            tsMapListTree.Enabled = true;

            //disable already enabled stuff in case a ROM had already been loaded
            mainTabControl.Enabled = false;

            //set some defaults
            mainTabControl.SelectedIndex = 0;
            glControlBlocks.Size = new Size(128, 64);
            glControlMapEditor.Size = new Size(64, 64);
        }

        public void EnableControlsOnMapLoad()
        {
            if (PGMEBackend.Program.currentLayout == null)
            {
                mainTabControl.Enabled = true;
            }

            editorTabLoaded = false;
        }

        private void mapNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapSortOrder("Name");
            ClearMapNodes();
            LoadMapNodes();
        }

        private void mapBankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapSortOrder("Bank");
            ClearMapNodes();
            LoadMapNodes();
        }

        private void mapLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapSortOrder("Layout");
            ClearMapNodes();
            LoadMapNodes();
        }

        private void mapTilesetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMapSortOrder("Tileset");
            ClearMapNodes();
            LoadMapNodes();
        }

        private void SetMapSortOrder(string order)
        {
            switch (order)
            {
                default:
                    tsddbMapSortOrder.Image = Properties.Resources.sort_alphabel_16x16;
                    mapNameToolStripMenuItem.Checked = true;
                    break;
                case "Bank":
                    tsddbMapSortOrder.Image = Properties.Resources.sort_number_16x16;
                    mapBankToolStripMenuItem.Checked = true;
                    break;
                case "Layout":
                    tsddbMapSortOrder.Image = Properties.Resources.sort_map_16x16;
                    mapLayoutToolStripMenuItem.Checked = true;
                    break;
                case "Tileset":
                    tsddbMapSortOrder.Image = Properties.Resources.sort_date_16x16;
                    mapTilesetToolStripMenuItem.Checked = true;
                    break;
            }
            settings.MapSortOrder = order;
            WriteConfig();
            SetCheckedMapSortOrder(order);
        }

        private void SetCheckedMapSortOrder(string order)
        {
            foreach (ToolStripMenuItem item in tsddbMapSortOrder.DropDownItems)
            {
                if (item.Tag.Equals(order))
                    item.Checked = true;
                else
                    item.Checked = false;
            }
        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        public void ClearMapNodes()
        {
            mapTreeNodes.Clear();
            mapListTreeView.Nodes.Clear();
            backupTree.Nodes.Clear();
        }

        TreeView backupTree = new TreeView();

        public void LoadMapNodes()
        {
            int i = 0;
            switch (settings.MapSortOrder)
            {
                default:
                    mapTreeNodes.Add(0xFF, new TreeNode(PGMEBackend.Program.rmInternalStrings.GetString("InvalidMapNameIndex")));
                    foreach (KeyValuePair<int, MapName> mapName in PGMEBackend.Program.mapNames)
                    {
                        var mapNameNode = new TreeNode("[" + mapName.Key.ToString("X2") + "] " + mapName.Value.name);
                        mapNameNode.SelectedImageKey = "Folder Closed";
                        mapNameNode.ImageKey = "Folder Closed";
                        backupTree.Nodes.Add(mapNameNode);
                        mapTreeNodes.Add(mapName.Key, mapNameNode);

                    }
                    foreach (MapBank mapBank in PGMEBackend.Program.mapBanks.Values)
                    {
                        foreach (Map map in mapBank.GetBank().Values)
                        {
                            try
                            {
                                var node = mapTreeNodes[map.mapNameIndex].Nodes.Add("mapNode" + i++, map.name);
                                node.Tag = map;
                                mapTreeNodes[map.mapNameIndex].SelectedImageKey = "Map Folder Closed";
                                mapTreeNodes[map.mapNameIndex].ImageKey = "Map Folder Closed";
                            }
                            catch (KeyNotFoundException)
                            {
                                if (!backupTree.Nodes.Contains(mapTreeNodes[0xFF]))
                                {
                                    backupTree.Nodes.Add(mapTreeNodes[0xFF]);
                                }
                                var node = mapTreeNodes[0xFF].Nodes.Add("mapNode" + i++, map.name);
                                node.Tag = map;
                                mapTreeNodes[0xFF].SelectedImageKey = "Map Folder Closed";
                                mapTreeNodes[0xFF].ImageKey = "Map Folder Closed";
                            }
                        }
                    }
                    break;
                case "Bank":
                    foreach (KeyValuePair<int, MapBank> mapBank in PGMEBackend.Program.mapBanks)
                    {
                        var bankNode = new TreeNode("[" + mapBank.Key.ToString("X2") + "]");
                        bankNode.SelectedImageKey = "Folder Closed";
                        bankNode.ImageKey = "Folder Closed";
                        backupTree.Nodes.Add(bankNode);
                        mapTreeNodes.Add(mapBank.Key, bankNode);
                        foreach (Map map in mapBank.Value.GetBank().Values)
                        {
                            var node = bankNode.Nodes.Add("mapNode" + i++, map.name);
                            node.Tag = map;
                            bankNode.SelectedImageKey = "Map Folder Closed";
                            bankNode.ImageKey = "Map Folder Closed";
                        }
                    }
                    break;
                case "Layout":
                    foreach (KeyValuePair<int, MapLayout> mapLayout in PGMEBackend.Program.mapLayouts)
                    {
                        var mapLayoutNode = new TreeNode(mapLayout.Value.name);
                        mapLayoutNode.Tag = mapLayout.Value;
                        backupTree.Nodes.Add(mapLayoutNode);
                        mapTreeNodes.Add(mapLayout.Key, mapLayoutNode);
                    }
                    foreach (MapBank mapBank in PGMEBackend.Program.mapBanks.Values)
                    {
                        foreach (Map map in mapBank.GetBank().Values)
                        {
                            var node = mapTreeNodes[map.mapLayoutIndex].Nodes.Add("mapNode" + i++, map.name);
                            node.Tag = map;
                            mapTreeNodes[map.mapLayoutIndex].SelectedImageKey = "Map Folder Closed";
                            mapTreeNodes[map.mapLayoutIndex].ImageKey = "Map Folder Closed";
                        }

                    }
                    break;
                case "Tileset":
                    int j = 0;
                    foreach (KeyValuePair<int, MapTileset> mapTileset in PGMEBackend.Program.mapTilesets)
                    {
                        var mapTilesetNode = new TreeNode("[" + j++ + "] " + settings.HexPrefix + (mapTileset.Key + 0x8000000).ToString("X8"));
                        backupTree.Nodes.Add(mapTilesetNode);
                        mapTreeNodes.Add(mapTileset.Key, mapTilesetNode);
                    }
                    foreach (MapBank mapBank in PGMEBackend.Program.mapBanks.Values)
                    {
                        foreach (Map map in mapBank.GetBank().Values)
                        {
                            var node = mapTreeNodes[map.layout.globalTilesetPointer].Nodes.Add("mapNode" + i++, map.name);
                            node.Tag = map;
                            mapTreeNodes[map.layout.globalTilesetPointer].SelectedImageKey = "Map Folder Closed";
                            mapTreeNodes[map.layout.globalTilesetPointer].ImageKey = "Map Folder Closed";

                            node = mapTreeNodes[map.layout.localTilesetPointer].Nodes.Add("mapNode" + i++, map.name);
                            node.Tag = map;
                            mapTreeNodes[map.layout.localTilesetPointer].SelectedImageKey = "Map Folder Closed";
                            mapTreeNodes[map.layout.localTilesetPointer].ImageKey = "Map Folder Closed";
                        }

                    }
                    foreach (KeyValuePair<int, MapLayout> mapLayout in PGMEBackend.Program.mapLayouts)
                    {
                        TreeNode node;
                        if (mapTreeNodes.ContainsKey(mapLayout.Value.globalTilesetPointer) && GetNodeFromTag(mapLayout.Value, mapTreeNodes[mapLayout.Value.globalTilesetPointer]) == null)
                        {
                            node = mapTreeNodes[mapLayout.Value.globalTilesetPointer].Nodes.Add("mapNode" + i++, mapLayout.Value.name);
                            node.Tag = mapLayout.Value;
                            //mapTreeNodes.Add(mapLayout.Key, node);
                            mapTreeNodes[mapLayout.Value.globalTilesetPointer].SelectedImageKey = "Map Folder Closed";
                            mapTreeNodes[mapLayout.Value.globalTilesetPointer].ImageKey = "Map Folder Closed";
                        }
                        if (mapTreeNodes.ContainsKey(mapLayout.Value.localTilesetPointer) && GetNodeFromTag(mapLayout.Value, mapTreeNodes[mapLayout.Value.localTilesetPointer]) == null)
                        {
                            node = mapTreeNodes[mapLayout.Value.localTilesetPointer].Nodes.Add("mapNode" + i++, mapLayout.Value.name);
                            node.Tag = mapLayout.Value;
                            //mapTreeNodes.Add(mapLayout.Key, node);
                            mapTreeNodes[mapLayout.Value.localTilesetPointer].SelectedImageKey = "Map Folder Closed";
                            mapTreeNodes[mapLayout.Value.localTilesetPointer].ImageKey = "Map Folder Closed";
                        }
                    }
                    break;
            }
            CopyTreeNodes(backupTree, mapListTreeView);
            if (PGMEBackend.Program.currentLayout != null)
            {
                TreeNode itemNode = null;
                object tag = null;
                if (PGMEBackend.Program.currentMap != null)
                    tag = PGMEBackend.Program.currentMap;
                else
                    tag = PGMEBackend.Program.currentLayout;
                foreach (TreeNode node in mapListTreeView.Nodes)
                {
                    itemNode = GetNodeFromTag(tag, node);
                    if (itemNode != null)
                    {
                        itemNode.EnsureVisible();
                        itemNode.ImageKey = "Map Selected";
                        currentTreeNode = itemNode;
                        break;
                    }
                }
            }
        }

        public TreeNode GetNodeFromTag(object tag, TreeNode rootNode)
        {
            if (rootNode.Tag != null && rootNode.Tag.Equals(tag))
                return rootNode;
            foreach (TreeNode node in rootNode.Nodes)
            {
                TreeNode next = GetNodeFromTag(tag, node);
                if (next != null)
                    return next;
            }
            return null;
        }

        public void CopyTreeNodes(TreeView treeview1, TreeView treeview2)
        {
            TreeNode newTn;
            foreach (TreeNode tn in treeview1.Nodes)
            {
                newTn = (TreeNode)tn.Clone();
                newTn.Nodes.Clear();
                bool matchesFilter = newTn.Text.ToLower().Contains(tsMapFilter.Text.ToLower());
                if (CopyChildren(newTn, tn, matchesFilter) || matchesFilter)
                    treeview2.Nodes.Add(newTn);
            }
        }
        public bool CopyChildren(TreeNode parent, TreeNode original, bool forceCreate)
        {
            TreeNode newTn;
            bool shouldCreateParent = false;
            foreach (TreeNode tn in original.Nodes)
            {
                newTn = (TreeNode)tn.Clone();
                newTn.Nodes.Clear();
                bool matchesFilter = newTn.Text.ToLower().Contains(tsMapFilter.Text.ToLower()) || forceCreate;
                bool childrenWantParent = false;
                if (tn.Nodes.Count != 0)
                    childrenWantParent = CopyChildren(newTn, tn, matchesFilter);
                if (childrenWantParent || matchesFilter)
                    shouldCreateParent = true;
                if (childrenWantParent || matchesFilter)
                    parent.Nodes.Add(newTn);
            }
            return shouldCreateParent;
        }

        public string ShowFileOpenDialog(string title, string filter, bool multiselect)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = title;
            open.Filter = filter;
            open.Multiselect = multiselect;
            open.CheckFileExists = true;
            open.CheckPathExists = true;
            open.ShowDialog();
            return open.FileName;
        }

        public void SetTitleText(string title)
        {
            Text = title;
        }

        private void tsddbMapSortOrder_Click(object sender, EventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void mapListTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageKey = "Map Folder Open";
            e.Node.SelectedImageKey = "Map Folder Open";
        }

        private void mapListTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.ImageKey = "Map Folder Closed";
            e.Node.SelectedImageKey = "Map Folder Closed";
        }

        private void mapListTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //TreeNode node = ((TreeView)sender).SelectedNode;
            TreeNode node = e.Node;
            if(((TreeView)sender).SelectedNode == node)
                LoadMapFromNode(node);
        }

        private void mapListTreeView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void mapListTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (mapListTreeView.SelectedNode != null && e.KeyCode == Keys.Enter)
            {
                TreeNode node = ((TreeView)sender).SelectedNode;
                if (node.Nodes.Count == 0)
                {
                    LoadMapFromNode(node);
                }
                else if (!node.IsExpanded)
                    node.Expand();
                else
                    node.Collapse();
                e.SuppressKeyPress = true;      //needed to prevent sound
            }
        }

        void LoadMapFromNode(TreeNode node)
        {
            if (node != null && node.Nodes.Count == 0 && node.Tag != null)
            {
                if (PGMEBackend.Program.LoadMap(node.Tag) != 0)
                {
                    mapListTreeView.SelectedNode = currentTreeNode;
                    return;
                }
                if (currentTreeNode != null)
                {
                    currentTreeNode.ImageKey = "Map";
                    currentTreeNode.SelectedImageKey = "Map";
                }
                node.ImageKey = "Map Selected";
                node.SelectedImageKey = "Map Selected";
                currentTreeNode = node;
                node.EnsureVisible();
            }
        }
        
        public void LoadMap(object map)
        {
            switch (mainTabControl.SelectedIndex)
            {
                default:
                    LoadEditorTab(map);
                    break;
            }
        }

        public void LoadEditorTab(object maybeaMap)
        {
            MapLayout mapLayout;
            if (maybeaMap is Map)
                mapLayout = (maybeaMap as Map).layout;
            else
                mapLayout = maybeaMap as MapLayout;

            if (mapLayout == null)
                return;

            if (mapLayout.globalTileset != null)
                mapLayout.globalTileset.Initialize(mapLayout);
            if (mapLayout.localTileset != null)
                mapLayout.localTileset.Initialize(mapLayout);

            PGMEBackend.Program.glMapEditor.width = mapLayout.layoutWidth * 16;
            PGMEBackend.Program.glMapEditor.height = mapLayout.layoutHeight * 16;
            SetGLMapEditorSize(PGMEBackend.Program.glMapEditor.width, PGMEBackend.Program.glMapEditor.height);

            int blockChooserHeight = (int)Math.Ceiling(PGMEBackend.Program.currentGame.MainTSBlocks / 8.0d) * 16;
            if (mapLayout.localTileset != null && mapLayout.localTileset.blockSet != null)
                blockChooserHeight += (int)Math.Ceiling(mapLayout.localTileset.blockSet.blocks.Length / 8.0d) * 16;
            PGMEBackend.Program.glBlockChooser.height = blockChooserHeight;
            SetGLBlockChooserSize(PGMEBackend.Program.glBlockChooser.width, PGMEBackend.Program.glBlockChooser.height);

            SetGLBorderBlocksSize(mapLayout.borderWidth * 16, mapLayout.borderHeight * 16);
            
            RefreshMapEditorControl();
            RefreshBlockEditorControl();
            RefreshBorderBlocksControl();

            editorTabLoaded = true;
        }

        private void tsmiReloadROM_Click(object sender, EventArgs e)
        {
            PGMEBackend.Program.ReloadROM();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            QuitApplication(0);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(PGMEBackend.Program.ROM.Edited || PGMEBackend.Program.isEdited)
                e.Cancel = PGMEBackend.Program.UnsavedChangesQuitDialog() == "Cancel";
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void glControlMapEditor_Load(object sender, EventArgs e)
        {
            glControlMapEditor.MakeCurrent();
            PGMEBackend.Program.glMapEditor = new GLMapEditor(glControlMapEditor.Width, glControlMapEditor.Height);
        }
        
        private void glControlBlocks_Load(object sender, EventArgs e)
        {
            glControlBlocks.MakeCurrent();
            PGMEBackend.Program.glBlockChooser = new GLBlockChooser(glControlBlocks.Width, glControlBlocks.Height);
        }

        private void glControlMapEditor_Paint(object sender, PaintEventArgs e)
        {
            if (!PGMEBackend.Program.glMapEditor) // Play nice
                return;
            
            glControlMapEditor.MakeCurrent();
            PGMEBackend.Program.glMapEditor.Paint(glControlMapEditor.Width, glControlMapEditor.Height);
            glControlMapEditor.SwapBuffers();
        }
        
        private void glControlBlocks_Paint(object sender, PaintEventArgs e)
        {
            if (!PGMEBackend.Program.glBlockChooser) // Play nice
                return;
            
            glControlBlocks.MakeCurrent();
            PGMEBackend.Program.glBlockChooser.Paint(glControlBlocks.Width, glControlBlocks.Height);
            glControlBlocks.SwapBuffers();
        }
        
        public void SetGLMapEditorSize(int w, int h)
        {
            glControlMapEditor.Width = w;
            glControlMapEditor.Height = h;
        }

        public void SetGLBlockChooserSize(int w, int h)
        {
            glControlBlocks.Width = w;
            glControlBlocks.Height = h;
        }

        public void SetGLBorderBlocksSize(int w, int h)
        {
            int totalHeight = borderBlocksBox.Height + paintTabControl.Height;
            glControlBorderBlocks.Width = w;
            glControlBorderBlocks.Height = h;
            glControlBorderBlocks.Location = new Point(78 - (w /2), glControlBorderBlocks.Location.Y);
            borderBlocksBox.Size = new Size(borderBlocksBox.Size.Width, 24 + h);
            paintTabControl.Location = new Point(paintTabControl.Location.X, 30 + h);
            paintTabControl.Size = new Size(paintTabControl.Size.Width, totalHeight - borderBlocksBox.Height);
        }

        private void glControlMapEditor_MouseMove(object sender, MouseEventArgs e)
        {
            int oldX = PGMEBackend.Program.glMapEditor.mouseX;
            int oldY = PGMEBackend.Program.glMapEditor.mouseY;

            PGMEBackend.Program.glMapEditor.MouseMove(e.X, e.Y);

            if ((oldX != PGMEBackend.Program.glMapEditor.mouseX) || (oldY != PGMEBackend.Program.glMapEditor.mouseY))
            {
                RefreshMapEditorControl();
            }
        }

        private void glControlBlocks_MouseMove(object sender, MouseEventArgs e)
        {
            RefreshBlockEditorControl();
        }

        private void glControlMapEditor_MouseLeave(object sender, EventArgs e)
        {
            RefreshMapEditorControl();
        }
        
        private void glControlBlocks_MouseLeave(object sender, EventArgs e)
        {
            RefreshBlockEditorControl();
        }

        private void glControlMapEditor_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshMapEditorControl();
        }

        private void glControlBlocks_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshBlockEditorControl();
        }

        private void glControlMapEditor_MouseUp(object sender, MouseEventArgs e)
        {
            RefreshMapEditorControl();
        }

        private void glControlBlocks_MouseUp(object sender, MouseEventArgs e)
        {
            RefreshBlockEditorControl();
        }

        private void glControlMapEditor_MouseEnter(object sender, EventArgs e)
        {

        }
        
        private void glControlBlocks_MouseEnter(object sender, EventArgs e)
        {

        }

        public void RefreshMapEditorControl()
        {
            glControlMapEditor.Invalidate();
        }

        public void RefreshBlockEditorControl()
        {
            glControlBlocks.Invalidate();
        }

        public void RefreshBorderBlocksControl()
        {
            glControlBorderBlocks.Invalidate();
        }

        private void panel8_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshMapEditorControl();
        }

        public void ScrollBlockChooserToBlock(int blockNum)
        {
            using (Control c = new Control() { Parent = blockPaintPanel, Height = 16, Top = (blockNum / 8) * 16 + blockPaintPanel.AutoScrollPosition.Y })
            {
                blockPaintPanel.ScrollControlIntoView(c);
            }
        }

        public void ScrollPermChooserToPerm(int permNum)
        {
            using (Control c = new Control() { Parent = movementPaintPanel, Height = 16, Top = (permNum / 4) * 16 + movementPaintPanel.AutoScrollPosition.Y })
            {
                movementPaintPanel.ScrollControlIntoView(c);
            }
        }

        private void blockPaintPanel_Scroll(object sender, ScrollEventArgs e)
        {
            RefreshBlockEditorControl();
        }

        private void toolStripShowGrid_Click(object sender, EventArgs e)
        {
            ChangeGridState();
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeGridState();
        }

        private void toolStripSaveMap_Click(object sender, EventArgs e)
        {
            PGMEBackend.Program.SaveMap();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PGMEBackend.Program.SaveMap();
        }

        private void cboTimeofDayMap_SelectedIndexChanged(object sender, EventArgs e)
        {
            PGMEBackend.Program.timeOfDay = (sender as ToolStripComboBox).SelectedIndex;
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool oldValue = PGMEBackend.Program.showingPerms;
            if (mainTabControl.SelectedIndex == 0)
            {
                if (paintTabControl.SelectedIndex == 1)
                    PGMEBackend.Program.showingPerms = true;
                else
                    PGMEBackend.Program.showingPerms = false;
                if (!editorTabLoaded)
                    LoadEditorTab((PGMEBackend.Program.currentMap != null) ? PGMEBackend.Program.currentMap : (object)PGMEBackend.Program.currentLayout);
            }
            if(oldValue != PGMEBackend.Program.showingPerms)
                PGMEBackend.Program.glMapEditor.RedrawAllChunks();
        }

        private void toolStripEventsShowGrid_Click(object sender, EventArgs e)
        {
            ChangeGridState();
        }

        private void ChangeGridState()
        {
            WriteConfig();
            RefreshMapEditorControl();
            RefreshBlockEditorControl();
            RefreshBorderBlocksControl();
        }

        private void glControlBorderBlocks_Load(object sender, EventArgs e)
        {
            glControlBorderBlocks.MakeCurrent();
            PGMEBackend.Program.glBorderBlocks = new GLBorderBlocks(glControlBorderBlocks.Width, glControlBorderBlocks.Height);
        }

        private void glControlBorderBlocks_Paint(object sender, PaintEventArgs e)
        {
            if (!PGMEBackend.Program.glBorderBlocks) // Play nice
                return;

            glControlBorderBlocks.MakeCurrent();
            PGMEBackend.Program.glBorderBlocks.Paint(glControlBorderBlocks.Width, glControlBorderBlocks.Height);
            glControlBorderBlocks.SwapBuffers();
        }

        private void glControlBorderBlocks_KeyDown(object sender, KeyEventArgs e)
        {
            isControlPressed = e.Control;
        }

        private void glControlBorderBlocks_KeyUp(object sender, KeyEventArgs e)
        {
            isControlPressed = e.Control;
        }

        private void glControlBorderBlocks_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshBorderBlocksControl();
        }

        private void glControlBorderBlocks_MouseEnter(object sender, EventArgs e)
        {

        }

        private void glControlBorderBlocks_MouseLeave(object sender, EventArgs e)
        {
            PGMEBackend.Program.glBorderBlocks.MouseLeave();
            RefreshBorderBlocksControl();
        }

        private void glControlBorderBlocks_MouseMove(object sender, MouseEventArgs e)
        {
            int oldX = PGMEBackend.Program.glBorderBlocks.mouseX;
            int oldY = PGMEBackend.Program.glBorderBlocks.mouseY;

            PGMEBackend.Program.glBorderBlocks.MouseMove(e.X, e.Y);

            if ((oldX != PGMEBackend.Program.glBorderBlocks.mouseX) || (oldY != PGMEBackend.Program.glBorderBlocks.mouseY))
                RefreshBorderBlocksControl();
        }

        private void glControlBorderBlocks_MouseUp(object sender, MouseEventArgs e)
        {
            RefreshBorderBlocksControl();
        }

        private void paintTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PGMEBackend.Program.ChangePermsVisibility((sender as TabControl).SelectedIndex == 1);
            RefreshMapEditorControl();
        }

        bool isControlPressed = false;

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            isControlPressed = e.Control;
        }

        private void MainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            isControlPressed = e.Control;
        }

        public int permTransPreview = -1;

        public int PermTransPreviewValue()
        {
            return permTransPreview;
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            OpenSettingsWindow();
        }

        public void OpenSettingsWindow()
        {
            SettingsDialog permTransDialog = new SettingsDialog(this);
            DialogResult result = permTransDialog.ShowDialog();
            if (result != DialogResult.OK && PGMEBackend.Program.currentLayout != null)
            {
                PGMEBackend.Program.glMapEditor.RedrawAllChunks();
                RefreshMapEditorControl();
            }
            permTransPreview = -1;
        }

        public void PreviewPermTranslucency(int value)
        {
            permTransPreview = value;
            if (PGMEBackend.Program.currentLayout != null)
            {
                PGMEBackend.Program.glMapEditor.RedrawAllChunks();
                RefreshMapEditorControl();
            }
        }

        private void tsMapFilter_TextChanged(object sender, EventArgs e)
        {
            ClearMapNodes();
            LoadMapNodes();
        }

        private void mainTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {

        }
    }

    class GLPanel : Panel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            if (activeControl is OpenTK.GLControl)
                return this.AutoScrollPosition;
            else
                return base.ScrollToControl(activeControl);
        }
    }
}
