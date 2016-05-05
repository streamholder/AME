// Awesome Map Editor
// A map editor for GBA Pokémon games.

// Copyright (C) 2015 Diegoisawesome

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

namespace PGMEWindowsUI
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.splitMapListAndPaint = new System.Windows.Forms.SplitContainer();
            this.tsMapListTree = new System.Windows.Forms.ToolStrip();
            this.tsddbMapSortOrder = new System.Windows.Forms.ToolStripDropDownButton();
            this.mapNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapBankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapTilesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsMapFilter = new System.Windows.Forms.ToolStripTextBox();
            this.mapListTreeView = new System.Windows.Forms.TreeView();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.mapTabPage = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.borderBlocksBox = new System.Windows.Forms.GroupBox();
            this.glControlBorderBlocks = new OpenTK.GLControl();
            this.mapEditorPanel = new System.Windows.Forms.Panel();
            this.mapPaintPanel = new PGMEWindowsUI.GLPanel();
            this.glControlMapEditor = new OpenTK.GLControl();
            this.paintTabControl = new System.Windows.Forms.TabControl();
            this.blocksTabPage = new System.Windows.Forms.TabPage();
            this.blockPaintPanel = new PGMEWindowsUI.GLPanel();
            this.glControlBlocks = new OpenTK.GLControl();
            this.movementTabPage = new System.Windows.Forms.TabPage();
            this.movementPaintPanel = new PGMEWindowsUI.GLPanel();
            this.glControlPermsChooser = new OpenTK.GLControl();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripOpen = new System.Windows.Forms.ToolStripButton();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitMapListAndPaint)).BeginInit();
            this.splitMapListAndPaint.Panel1.SuspendLayout();
            this.splitMapListAndPaint.Panel2.SuspendLayout();
            this.splitMapListAndPaint.SuspendLayout();
            this.tsMapListTree.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.mapTabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.borderBlocksBox.SuspendLayout();
            this.mapEditorPanel.SuspendLayout();
            this.mapPaintPanel.SuspendLayout();
            this.paintTabControl.SuspendLayout();
            this.blocksTabPage.SuspendLayout();
            this.blockPaintPanel.SuspendLayout();
            this.movementTabPage.SuspendLayout();
            this.movementPaintPanel.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMapListAndPaint
            // 
            resources.ApplyResources(this.splitMapListAndPaint, "splitMapListAndPaint");
            this.splitMapListAndPaint.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMapListAndPaint.Name = "splitMapListAndPaint";
            // 
            // splitMapListAndPaint.Panel1
            // 
            this.splitMapListAndPaint.Panel1.Controls.Add(this.tsMapListTree);
            this.splitMapListAndPaint.Panel1.Controls.Add(this.mapListTreeView);
            resources.ApplyResources(this.splitMapListAndPaint.Panel1, "splitMapListAndPaint.Panel1");
            // 
            // splitMapListAndPaint.Panel2
            // 
            this.splitMapListAndPaint.Panel2.Controls.Add(this.mainTabControl);
            // 
            // tsMapListTree
            // 
            resources.ApplyResources(this.tsMapListTree, "tsMapListTree");
            this.tsMapListTree.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMapListTree.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsMapListTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbMapSortOrder,
            this.tsMapFilter});
            this.tsMapListTree.Name = "tsMapListTree";
            // 
            // tsddbMapSortOrder
            // 
            this.tsddbMapSortOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbMapSortOrder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapNameToolStripMenuItem,
            this.mapBankToolStripMenuItem,
            this.mapLayoutToolStripMenuItem,
            this.mapTilesetToolStripMenuItem});
            resources.ApplyResources(this.tsddbMapSortOrder, "tsddbMapSortOrder");
            this.tsddbMapSortOrder.Name = "tsddbMapSortOrder";
            // 
            // mapNameToolStripMenuItem
            // 
            this.mapNameToolStripMenuItem.CheckOnClick = true;
            resources.ApplyResources(this.mapNameToolStripMenuItem, "mapNameToolStripMenuItem");
            this.mapNameToolStripMenuItem.Name = "mapNameToolStripMenuItem";
            this.mapNameToolStripMenuItem.Tag = "Name";
            // 
            // mapBankToolStripMenuItem
            // 
            this.mapBankToolStripMenuItem.CheckOnClick = true;
            resources.ApplyResources(this.mapBankToolStripMenuItem, "mapBankToolStripMenuItem");
            this.mapBankToolStripMenuItem.Name = "mapBankToolStripMenuItem";
            this.mapBankToolStripMenuItem.Tag = "Bank";
            // 
            // mapLayoutToolStripMenuItem
            // 
            this.mapLayoutToolStripMenuItem.Image = global::PGMEWindowsUI.Properties.Resources.sort_map_16x16;
            this.mapLayoutToolStripMenuItem.Name = "mapLayoutToolStripMenuItem";
            resources.ApplyResources(this.mapLayoutToolStripMenuItem, "mapLayoutToolStripMenuItem");
            this.mapLayoutToolStripMenuItem.Tag = "Layout";
            // 
            // mapTilesetToolStripMenuItem
            // 
            this.mapTilesetToolStripMenuItem.CheckOnClick = true;
            resources.ApplyResources(this.mapTilesetToolStripMenuItem, "mapTilesetToolStripMenuItem");
            this.mapTilesetToolStripMenuItem.Name = "mapTilesetToolStripMenuItem";
            this.mapTilesetToolStripMenuItem.Tag = "Tileset";
            // 
            // tsMapFilter
            // 
            this.tsMapFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsMapFilter.Name = "tsMapFilter";
            resources.ApplyResources(this.tsMapFilter, "tsMapFilter");
            this.tsMapFilter.TextChanged += new System.EventHandler(this.tsMapFilter_TextChanged);
            // 
            // mapListTreeView
            // 
            resources.ApplyResources(this.mapListTreeView, "mapListTreeView");
            this.mapListTreeView.Name = "mapListTreeView";
            this.mapListTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mapListTreeView_NodeMouseDoubleClick);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.mapTabPage);
            resources.ApplyResources(this.mainTabControl, "mainTabControl");
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            // 
            // mapTabPage
            // 
            this.mapTabPage.Controls.Add(this.panel2);
            resources.ApplyResources(this.mapTabPage, "mapTabPage");
            this.mapTabPage.Name = "mapTabPage";
            this.mapTabPage.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.borderBlocksBox);
            this.panel2.Controls.Add(this.mapEditorPanel);
            this.panel2.Controls.Add(this.paintTabControl);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // borderBlocksBox
            // 
            resources.ApplyResources(this.borderBlocksBox, "borderBlocksBox");
            this.borderBlocksBox.Controls.Add(this.glControlBorderBlocks);
            this.borderBlocksBox.Name = "borderBlocksBox";
            this.borderBlocksBox.TabStop = false;
            // 
            // glControlBorderBlocks
            // 
            resources.ApplyResources(this.glControlBorderBlocks, "glControlBorderBlocks");
            this.glControlBorderBlocks.BackColor = System.Drawing.Color.Black;
            this.glControlBorderBlocks.Name = "glControlBorderBlocks";
            this.glControlBorderBlocks.VSync = false;
            this.glControlBorderBlocks.Load += new System.EventHandler(this.glControlBorderBlocks_Load);
            this.glControlBorderBlocks.Paint += new System.Windows.Forms.PaintEventHandler(this.glControlBorderBlocks_Paint);
            // 
            // mapEditorPanel
            // 
            resources.ApplyResources(this.mapEditorPanel, "mapEditorPanel");
            this.mapEditorPanel.Controls.Add(this.mapPaintPanel);
            this.mapEditorPanel.Name = "mapEditorPanel";
            // 
            // mapPaintPanel
            // 
            resources.ApplyResources(this.mapPaintPanel, "mapPaintPanel");
            this.mapPaintPanel.BackColor = System.Drawing.Color.Transparent;
            this.mapPaintPanel.Controls.Add(this.glControlMapEditor);
            this.mapPaintPanel.Name = "mapPaintPanel";
            // 
            // glControlMapEditor
            // 
            this.glControlMapEditor.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.glControlMapEditor.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.glControlMapEditor, "glControlMapEditor");
            this.glControlMapEditor.Name = "glControlMapEditor";
            this.glControlMapEditor.VSync = false;
            this.glControlMapEditor.Load += new System.EventHandler(this.glControlMapEditor_Load);
            this.glControlMapEditor.Paint += new System.Windows.Forms.PaintEventHandler(this.glControlMapEditor_Paint);
            // 
            // paintTabControl
            // 
            resources.ApplyResources(this.paintTabControl, "paintTabControl");
            this.paintTabControl.Controls.Add(this.blocksTabPage);
            this.paintTabControl.Controls.Add(this.movementTabPage);
            this.paintTabControl.Name = "paintTabControl";
            this.paintTabControl.SelectedIndex = 0;
            // 
            // blocksTabPage
            // 
            this.blocksTabPage.Controls.Add(this.blockPaintPanel);
            resources.ApplyResources(this.blocksTabPage, "blocksTabPage");
            this.blocksTabPage.Name = "blocksTabPage";
            this.blocksTabPage.UseVisualStyleBackColor = true;
            // 
            // blockPaintPanel
            // 
            resources.ApplyResources(this.blockPaintPanel, "blockPaintPanel");
            this.blockPaintPanel.BackColor = System.Drawing.Color.Transparent;
            this.blockPaintPanel.Controls.Add(this.glControlBlocks);
            this.blockPaintPanel.Name = "blockPaintPanel";
            // 
            // glControlBlocks
            // 
            this.glControlBlocks.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.glControlBlocks, "glControlBlocks");
            this.glControlBlocks.Name = "glControlBlocks";
            this.glControlBlocks.VSync = false;
            this.glControlBlocks.Load += new System.EventHandler(this.glControlBlocks_Load);
            this.glControlBlocks.Paint += new System.Windows.Forms.PaintEventHandler(this.glControlBlocks_Paint);
            // 
            // movementTabPage
            // 
            this.movementTabPage.Controls.Add(this.movementPaintPanel);
            resources.ApplyResources(this.movementTabPage, "movementTabPage");
            this.movementTabPage.Name = "movementTabPage";
            this.movementTabPage.UseVisualStyleBackColor = true;
            // 
            // movementPaintPanel
            // 
            resources.ApplyResources(this.movementPaintPanel, "movementPaintPanel");
            this.movementPaintPanel.Controls.Add(this.glControlPermsChooser);
            this.movementPaintPanel.Name = "movementPaintPanel";
            // 
            // glControlPermsChooser
            // 
            this.glControlPermsChooser.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.glControlPermsChooser, "glControlPermsChooser");
            this.glControlPermsChooser.Name = "glControlPermsChooser";
            this.glControlPermsChooser.VSync = false;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen});
            resources.ApplyResources(this.mainToolStrip, "mainToolStrip");
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Stretch = true;
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripOpen, "toolStripOpen");
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Click += new System.EventHandler(this.toolStripOpen_Click);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            resources.ApplyResources(this.mainStatusStrip, "mainStatusStrip");
            this.mainStatusStrip.Name = "mainStatusStrip";
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitMapListAndPaint);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainToolStrip);
            this.KeyPreview = true;
            this.Name = "MainWindow";
            this.splitMapListAndPaint.Panel1.ResumeLayout(false);
            this.splitMapListAndPaint.Panel1.PerformLayout();
            this.splitMapListAndPaint.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMapListAndPaint)).EndInit();
            this.splitMapListAndPaint.ResumeLayout(false);
            this.tsMapListTree.ResumeLayout(false);
            this.tsMapListTree.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.mapTabPage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.borderBlocksBox.ResumeLayout(false);
            this.mapEditorPanel.ResumeLayout(false);
            this.mapPaintPanel.ResumeLayout(false);
            this.paintTabControl.ResumeLayout(false);
            this.blocksTabPage.ResumeLayout(false);
            this.blockPaintPanel.ResumeLayout(false);
            this.movementTabPage.ResumeLayout(false);
            this.movementPaintPanel.ResumeLayout(false);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripOpen;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.TreeView mapListTreeView;
        private System.Windows.Forms.SplitContainer splitMapListAndPaint;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage mapTabPage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox borderBlocksBox;
        private OpenTK.GLControl glControlBorderBlocks;
        private System.Windows.Forms.Panel mapEditorPanel;
        private GLPanel mapPaintPanel;
        private OpenTK.GLControl glControlMapEditor;
        private System.Windows.Forms.TabControl paintTabControl;
        private System.Windows.Forms.TabPage blocksTabPage;
        private GLPanel blockPaintPanel;
        private OpenTK.GLControl glControlBlocks;
        private System.Windows.Forms.TabPage movementTabPage;
        private GLPanel movementPaintPanel;
        private OpenTK.GLControl glControlPermsChooser;
        private System.Windows.Forms.ToolStrip tsMapListTree;
        private System.Windows.Forms.ToolStripDropDownButton tsddbMapSortOrder;
        private System.Windows.Forms.ToolStripMenuItem mapNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapBankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapTilesetToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox tsMapFilter;
    }
}

