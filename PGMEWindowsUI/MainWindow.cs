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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PGMEBackend;
using PGMEBackend.GLControls;

namespace PGMEWindowsUI
{
    public partial class MainWindow : Form, UIInteractionLayer
    {
        public Dictionary<int, TreeNode> mapTreeNodes;
        TreeNode currentTreeNode;

        public MainWindow()
        {
            PGMEBackend.Program.Initialize(this);

            InitializeComponent();

            PGMEBackend.Program.SetMainGUITitle(this.Text);
            mapTreeNodes = new Dictionary<int, TreeNode>();
        }

        public IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
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

        public void EnableControlsOnROMLoad()
        {
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
            mapTreeNodes.Add(0xFF, new TreeNode("InvalidMapNameIndex"));
            foreach (KeyValuePair<int, String> mapName in PGMEBackend.Program.mapNames)
            {
                var mapNameNode = new TreeNode("[" + mapName.Key.ToString("X2") + "] " + mapName.Value);
                backupTree.Nodes.Add(mapNameNode);
                mapTreeNodes.Add(mapName.Key, mapNameNode);

            }
            foreach (MapBank mapBank in PGMEBackend.Program.mapBanks.Values)
            {
                foreach (Map map in mapBank.GetBank().Values)
                {
                    var node = mapTreeNodes[map.mapNameIndex].Nodes.Add("mapNode" + i++, map.name);
                    node.Tag = map;
                }
            }

            CopyTreeNodes(backupTree, mapListTreeView);
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

        private void mapListTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //TreeNode node = ((TreeView)sender).SelectedNode;
            TreeNode node = e.Node;
            if(((TreeView)sender).SelectedNode == node)
                LoadMapFromNode(node);
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

            glControlMapEditor.Invalidate();
            glControlBlocks.Invalidate();
            glControlBorderBlocks.Invalidate();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            QuitApplication(0);
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

        private void tsMapFilter_TextChanged(object sender, EventArgs e)
        {
            ClearMapNodes();
            LoadMapNodes();
        }

        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            PGMEBackend.Program.LoadROM();
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
