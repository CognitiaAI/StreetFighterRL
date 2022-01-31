﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk
{
	public partial class InputRoll
	{
		protected override void OnPaint(PaintEventArgs e)
		{
			using (_gdi.LockGraphics(e.Graphics))
			{
				_gdi.StartOffScreenBitmap(Width, Height);

				// White Background
				_gdi.SetBrush(Color.White);
				_gdi.SetSolidPen(Color.White);
				_gdi.FillRectangle(0, 0, Width, Height);

				// Lag frame calculations
				SetLagFramesArray();

				var visibleColumns = _columns.VisibleColumns.ToList();

				if (visibleColumns.Any())
				{
					DrawColumnBg(e, visibleColumns);
					DrawColumnText(e, visibleColumns);
				}

				// Background
				DrawBg(e, visibleColumns);

				// Foreground
				DrawData(e, visibleColumns);

				DrawColumnDrag(e);
				DrawCellDrag(e);

				_gdi.CopyToScreen();
				_gdi.EndOffScreenBitmap();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Do nothing, and this should never be called
		}

		private void DrawColumnDrag(PaintEventArgs e)
		{
			if (_columnDown != null && _columnDownMoved && _currentX.HasValue && _currentY.HasValue && IsHoveringOnColumnCell)
			{
				int x1 = _currentX.Value - (_columnDown.Width.Value / 2);
				int y1 = _currentY.Value - (CellHeight / 2);
				int x2 = x1 + _columnDown.Width.Value;
				int y2 = y1 + CellHeight;

				_gdi.SetSolidPen(_backColor);
				_gdi.DrawRectangle(x1, y1, x2, y2);
				_gdi.PrepDrawString(_normalFont, _foreColor);
				_gdi.DrawString(_columnDown.Text, new Point(x1 + CellWidthPadding, y1 + CellHeightPadding));
			}
		}

		private void DrawCellDrag(PaintEventArgs e)
		{
			if (_draggingCell != null)
			{
				var text = "";
				int offsetX = 0;
				int offsetY = 0;
				QueryItemText?.Invoke(_draggingCell.RowIndex.Value, _draggingCell.Column, out text, ref offsetX, ref offsetY);

				Color bgColor = _backColor;
				QueryItemBkColor?.Invoke(_draggingCell.RowIndex.Value, _draggingCell.Column, ref bgColor);

				int x1 = _currentX.Value - (_draggingCell.Column.Width.Value / 2);
				int y1 = _currentY.Value - (CellHeight / 2);
				int x2 = x1 + _draggingCell.Column.Width.Value;
				int y2 = y1 + CellHeight;


				_gdi.SetBrush(bgColor);
				_gdi.FillRectangle(x1, y1, x2 - x1, y2 - y1);
				_gdi.PrepDrawString(_normalFont, _foreColor);
				_gdi.DrawString(text, new Point(x1 + CellWidthPadding + offsetX, y1 + CellHeightPadding + offsetY));
			}
		}

		private void DrawColumnText(PaintEventArgs e, List<RollColumn> visibleColumns)
		{
			if (HorizontalOrientation)
			{
				int start = -_vBar.Value;

				_gdi.PrepDrawString(_normalFont, _foreColor);

				foreach (var column in visibleColumns)
				{
					var point = new Point(CellWidthPadding, start + CellHeightPadding);

					if (IsHoveringOnColumnCell && column == CurrentCell.Column)
					{
						_gdi.PrepDrawString(_normalFont, SystemColors.HighlightText);
						_gdi.DrawString(column.Text, point);
						_gdi.PrepDrawString(_normalFont, _foreColor);
					}
					else
					{
						_gdi.DrawString(column.Text, point);
					}

					start += CellHeight;
				}
			}
			else
			{
				_gdi.PrepDrawString(_normalFont, _foreColor);

				foreach (var column in visibleColumns)
				{
					var point = new Point(column.Left.Value + 2 * CellWidthPadding - _hBar.Value, CellHeightPadding); // TODO: fix this CellPadding issue (2 * CellPadding vs just CellPadding)

					if (IsHoveringOnColumnCell && column == CurrentCell.Column)
					{
						_gdi.PrepDrawString(_normalFont, SystemColors.HighlightText);
						_gdi.DrawString(column.Text, point);
						_gdi.PrepDrawString(_normalFont, _foreColor);
					}
					else
					{
						_gdi.DrawString(column.Text, point);
					}
				}
			}
		}

		private void DrawData(PaintEventArgs e, List<RollColumn> visibleColumns)
		{
			if (QueryItemText != null)
			{
				if (HorizontalOrientation)
				{
					int startRow = FirstVisibleRow;
					int range = Math.Min(LastVisibleRow, RowCount - 1) - startRow + 1;

					_gdi.PrepDrawString(_normalFont, _foreColor);
					for (int i = 0, f = 0; f < range; i++, f++)
					{
						f += _lagFrames[i];
						int LastVisible = LastVisibleColumnIndex;
						for (int j = FirstVisibleColumn; j <= LastVisible; j++)
						{
							Bitmap image = null;
							int x = 0;
							int y = 0;
							int bitmapOffsetX = 0;
							int bitmapOffsetY = 0;

							QueryItemIcon?.Invoke(f + startRow, visibleColumns[j], ref image, ref bitmapOffsetX, ref bitmapOffsetY);

							if (image != null)
							{
								x = RowsToPixels(i) + CellWidthPadding + bitmapOffsetX;
								y = (j * CellHeight) + (CellHeightPadding * 2) + bitmapOffsetY;
								_gdi.DrawBitmap(image, new Point(x, y), true);
							}

							string text;
							int strOffsetX = 0;
							int strOffsetY = 0;
							QueryItemText(f + startRow, visibleColumns[j], out text, ref strOffsetX, ref strOffsetY);

							// Center Text
							x = RowsToPixels(i) + ((CellWidth - (text.Length * _charSize.Width)) / 2);
							y = (j * CellHeight) + CellHeightPadding - _vBar.Value;
							var point = new Point(x + strOffsetX, y + strOffsetY);

							var rePrep = false;
							if (j == 1)
							if (_selectedItems.Contains(new Cell { Column = visibleColumns[j], RowIndex = i + startRow }))
							{
								_gdi.PrepDrawString(_rotatedFont, SystemColors.HighlightText);
								rePrep = true;
							}
							else if (j == 1)
							{
								// 1. not sure about this; 2. repreps may be excess, but if we render one column at a time, we do need to change back after rendering the header
								rePrep = true;
								_gdi.PrepDrawString(_rotatedFont, _foreColor);
							}

							if (!string.IsNullOrWhiteSpace(text))
							{
								_gdi.DrawString(text, point);
							}

							if (rePrep)
							{
								_gdi.PrepDrawString(_normalFont, _foreColor);
							}
						}
					}
				}
				else
				{
					int startRow = FirstVisibleRow;
					int range = Math.Min(LastVisibleRow, RowCount - 1) - startRow + 1;

					_gdi.PrepDrawString(_normalFont, _foreColor);
					int xPadding = CellWidthPadding + 1 - _hBar.Value;
					for (int i = 0, f = 0; f < range; i++, f++) // Vertical
					{
						f += _lagFrames[i];
						int LastVisible = LastVisibleColumnIndex;
						for (int j = FirstVisibleColumn; j <= LastVisible; j++) // Horizontal
						{
							RollColumn col = visibleColumns[j];

							string text;
							int strOffsetX = 0;
							int strOffsetY = 0;
							Point point = new Point(col.Left.Value + xPadding, RowsToPixels(i) + CellHeightPadding);

							Bitmap image = null;
							int bitmapOffsetX = 0;
							int bitmapOffsetY = 0;

							QueryItemIcon?.Invoke(f + startRow, visibleColumns[j], ref image, ref bitmapOffsetX, ref bitmapOffsetY);

							if (image != null)
							{
								_gdi.DrawBitmap(image, new Point(point.X + bitmapOffsetX, point.Y + bitmapOffsetY + CellHeightPadding), true);
							}

							QueryItemText(f + startRow, visibleColumns[j], out text, ref strOffsetX, ref strOffsetY);

							bool rePrep = false;
							if (_selectedItems.Contains(new Cell { Column = visibleColumns[j], RowIndex = f + startRow }))
							{
								_gdi.PrepDrawString(_normalFont, SystemColors.HighlightText);
								rePrep = true;
							}

							if (!string.IsNullOrWhiteSpace(text))
							{
								_gdi.DrawString(text, new Point(point.X + strOffsetX, point.Y + strOffsetY));
							}

							if (rePrep)
							{
								_gdi.PrepDrawString(_normalFont, _foreColor);
							}
						}
					}
				}
			}
		}

		private void DrawColumnBg(PaintEventArgs e, List<RollColumn> visibleColumns)
		{
			_gdi.SetBrush(SystemColors.ControlLight);
			_gdi.SetSolidPen(Color.Black);

			if (HorizontalOrientation)
			{
				_gdi.FillRectangle(0, 0, ColumnWidth + 1, DrawHeight + 1);
				_gdi.Line(0, 0, 0, visibleColumns.Count * CellHeight + 1);
				_gdi.Line(ColumnWidth, 0, ColumnWidth, visibleColumns.Count * CellHeight + 1);

				int start = -_vBar.Value;
				foreach (var column in visibleColumns)
				{
					_gdi.Line(1, start, ColumnWidth, start);
					start += CellHeight;
				}

				if (visibleColumns.Any())
				{
					_gdi.Line(1, start, ColumnWidth, start);
				}
			}
			else
			{
				int bottomEdge = RowsToPixels(0);

				// Gray column box and black line underneath
				_gdi.FillRectangle(0, 0, Width + 1, bottomEdge + 1);
				_gdi.Line(0, 0, TotalColWidth.Value + 1, 0);
				_gdi.Line(0, bottomEdge, TotalColWidth.Value + 1, bottomEdge);

				// Vertical black seperators
				for (int i = 0; i < visibleColumns.Count; i++)
				{
					int pos = visibleColumns[i].Left.Value - _hBar.Value;
					_gdi.Line(pos, 0, pos, bottomEdge);
				}

				// Draw right most line
				if (visibleColumns.Any())
				{
					int right = TotalColWidth.Value - _hBar.Value;
					_gdi.Line(right, 0, right, bottomEdge);
				}
			}

			// Emphasis
			foreach (var column in visibleColumns.Where(c => c.Emphasis))
			{
				_gdi.SetBrush(SystemColors.ActiveBorder);
				if (HorizontalOrientation)
				{
					_gdi.FillRectangle(1, visibleColumns.IndexOf(column) * CellHeight + 1, ColumnWidth - 1, ColumnHeight - 1);
				}
				else
				{
					_gdi.FillRectangle(column.Left.Value + 1 - _hBar.Value, 1, column.Width.Value - 1, ColumnHeight - 1);
				}
			}

			// If the user is hovering over a column
			if (IsHoveringOnColumnCell)
			{
				if (HorizontalOrientation)
				{
					for (int i = 0; i < visibleColumns.Count; i++)
					{
						if (visibleColumns[i] != CurrentCell.Column)
						{
							continue;
						}

						if (CurrentCell.Column.Emphasis)
						{
							_gdi.SetBrush(Add(SystemColors.Highlight, 0x00222222));
						}
						else
						{
							_gdi.SetBrush(SystemColors.Highlight);
						}

						_gdi.FillRectangle(1, i * CellHeight + 1, ColumnWidth - 1, ColumnHeight - 1);
					}
				}
				else
				{
					// TODO multiple selected columns
					for (int i = 0; i < visibleColumns.Count; i++)
					{
						if (visibleColumns[i] == CurrentCell.Column)
						{
							// Left of column is to the right of the viewable area or right of column is to the left of the viewable area
							if (visibleColumns[i].Left.Value - _hBar.Value > Width || visibleColumns[i].Right.Value - _hBar.Value < 0)
							{
								continue;
							}

							int left = visibleColumns[i].Left.Value - _hBar.Value;
							int width = visibleColumns[i].Right.Value - _hBar.Value - left;

							if (CurrentCell.Column.Emphasis)
							{
								_gdi.SetBrush(Add(SystemColors.Highlight, 0x00550000));
							}
							else
							{
								_gdi.SetBrush(SystemColors.Highlight);
							}

							_gdi.FillRectangle(left + 1, 1, width - 1, ColumnHeight - 1);
						}
					}
				}
			}
		}

		// TODO refactor this and DoBackGroundCallback functions.
		/// <summary>
		/// Draw Gridlines and background colors using QueryItemBkColor.
		/// </summary>
		private void DrawBg(PaintEventArgs e, List<RollColumn> visibleColumns)
		{
			if (UseCustomBackground && QueryItemBkColor != null)
			{
				DoBackGroundCallback(e, visibleColumns);
			}

			if (GridLines)
			{
				_gdi.SetSolidPen(SystemColors.ControlLight);
				if (HorizontalOrientation)
				{
					// Columns
					for (int i = 1; i < VisibleRows + 1; i++)
					{
						int x = RowsToPixels(i);
						_gdi.Line(x, 1, x, DrawHeight);
					}

					// Rows
					for (int i = 0; i < visibleColumns.Count + 1; i++)
					{
						_gdi.Line(RowsToPixels(0) + 1, i * CellHeight - _vBar.Value, DrawWidth, i * CellHeight - _vBar.Value);
					}
				}
				else
				{
					// Columns
					int y = ColumnHeight + 1;
					int? totalColWidth = TotalColWidth;
					foreach (var column in visibleColumns)
					{
						int x = column.Left.Value - _hBar.Value;
						_gdi.Line(x, y, x, Height - 1);
					}

					if (visibleColumns.Any())
					{
						_gdi.Line(totalColWidth.Value - _hBar.Value, y, totalColWidth.Value - _hBar.Value, Height - 1);
					}

					// Rows
					for (int i = 1; i < VisibleRows + 1; i++)
					{
						_gdi.Line(0, RowsToPixels(i), Width + 1, RowsToPixels(i));
					}
				}
			}

			if (_selectedItems.Any())
			{
				DoSelectionBG(e, visibleColumns);
			}
		}

		private void DoSelectionBG(PaintEventArgs e, List<RollColumn> visibleColumns)
		{
			// SuuperW: This allows user to see other colors in selected frames.
			Color rowColor = Color.White;
			int _lastVisibleRow = LastVisibleRow;
			int lastRow = -1;
			foreach (Cell cell in _selectedItems)
			{
				if (cell.RowIndex > _lastVisibleRow || cell.RowIndex < FirstVisibleRow || !VisibleColumns.Contains(cell.Column))
				{
					continue;
				}

				Cell relativeCell = new Cell
				{
					RowIndex = cell.RowIndex - FirstVisibleRow,
					Column = cell.Column,
				};
				relativeCell.RowIndex -= CountLagFramesAbsolute(relativeCell.RowIndex.Value);

				if (QueryRowBkColor != null && lastRow != cell.RowIndex.Value)
				{
					QueryRowBkColor(cell.RowIndex.Value, ref rowColor);
					lastRow = cell.RowIndex.Value;
				}

				Color cellColor = rowColor;
				QueryItemBkColor(cell.RowIndex.Value, cell.Column, ref cellColor);

				// Alpha layering for cell before selection
				float alpha = (float)cellColor.A / 255;
				if (cellColor.A != 255 && cellColor.A != 0)
				{
					cellColor = Color.FromArgb(rowColor.R - (int)((rowColor.R - cellColor.R) * alpha),
						rowColor.G - (int)((rowColor.G - cellColor.G) * alpha),
						rowColor.B - (int)((rowColor.B - cellColor.B) * alpha));
				}

				// Alpha layering for selection
				alpha = 0.33f;
				cellColor = Color.FromArgb(cellColor.R - (int)((cellColor.R - SystemColors.Highlight.R) * alpha),
					cellColor.G - (int)((cellColor.G - SystemColors.Highlight.G) * alpha),
					cellColor.B - (int)((cellColor.B - SystemColors.Highlight.B) * alpha));
				DrawCellBG(cellColor, relativeCell, visibleColumns);
			}
		}

		/// <summary>
		/// Given a cell with rowindex inbetween 0 and VisibleRows, it draws the background color specified. Do not call with absolute rowindices.
		/// </summary>
		private void DrawCellBG(Color color, Cell cell, List<RollColumn> visibleColumns)
		{
			int x, y, w, h;

			if (HorizontalOrientation)
			{
				x = RowsToPixels(cell.RowIndex.Value) + 1;
				w = CellWidth - 1;
				y = (CellHeight * visibleColumns.IndexOf(cell.Column)) + 1 - _vBar.Value; // We can't draw without row and column, so assume they exist and fail catastrophically if they don't
				h = CellHeight - 1;
				if (x < ColumnWidth)
				{
					return;
				}
			}
			else
			{
				w = cell.Column.Width.Value - 1;
				x = cell.Column.Left.Value - _hBar.Value + 1;
				y = RowsToPixels(cell.RowIndex.Value) + 1; // We can't draw without row and column, so assume they exist and fail catastrophically if they don't
				h = CellHeight - 1;
				if (y < ColumnHeight)
				{
					return;
				}
			}

			if (x > DrawWidth || y > DrawHeight)
			{
				return;
			} // Don't draw if off screen.

			_gdi.SetBrush(color);
			_gdi.FillRectangle(x, y, w, h);
		}

		/// <summary>
		/// Calls QueryItemBkColor callback for all visible cells and fills in the background of those cells.
		/// </summary>
		/// <param name="e"></param>
		private void DoBackGroundCallback(PaintEventArgs e, List<RollColumn> visibleColumns)
		{
			int startIndex = FirstVisibleRow;
			int range = Math.Min(LastVisibleRow, RowCount - 1) - startIndex + 1;
			int lastVisible = LastVisibleColumnIndex;
			int firstVisibleColumn = FirstVisibleColumn;
			if (HorizontalOrientation)
			{
				for (int i = 0, f = 0; f < range; i++, f++)
				{
					f += _lagFrames[i];
					
					Color rowColor = Color.White;
					QueryRowBkColor?.Invoke(f + startIndex, ref rowColor);

					for (int j = firstVisibleColumn; j <= lastVisible; j++)
					{
						Color itemColor = Color.White;
						QueryItemBkColor(f + startIndex, visibleColumns[j], ref itemColor);
						if (itemColor == Color.White)
						{
							itemColor = rowColor;
						}
						else if (itemColor.A != 255 && itemColor.A != 0)
						{
							float alpha = (float)itemColor.A / 255;
							itemColor = Color.FromArgb(rowColor.R - (int)((rowColor.R - itemColor.R) * alpha),
								rowColor.G - (int)((rowColor.G - itemColor.G) * alpha),
								rowColor.B - (int)((rowColor.B - itemColor.B) * alpha));
						}

						if (itemColor != Color.White) // An easy optimization, don't draw unless the user specified something other than the default
						{
							var cell = new Cell
							{
								Column = visibleColumns[j],
								RowIndex = i
							};
							DrawCellBG(itemColor, cell, visibleColumns);
						}
					}
				}
			}
			else
			{
				for (int i = 0, f = 0; f < range; i++, f++) // Vertical
				{
					f += _lagFrames[i];
					
					Color rowColor = Color.White;
					QueryRowBkColor?.Invoke(f + startIndex, ref rowColor);

					for (int j = FirstVisibleColumn; j <= lastVisible; j++) // Horizontal
					{
						Color itemColor = Color.White;
						QueryItemBkColor(f + startIndex, visibleColumns[j], ref itemColor);
						if (itemColor == Color.White)
						{
							itemColor = rowColor;
						}
						else if (itemColor.A != 255 && itemColor.A != 0)
						{
							float alpha = (float)itemColor.A / 255;
							itemColor = Color.FromArgb(rowColor.R - (int)((rowColor.R - itemColor.R) * alpha),
								rowColor.G - (int)((rowColor.G - itemColor.G) * alpha),
								rowColor.B - (int)((rowColor.B - itemColor.B) * alpha));
						}

						if (itemColor != Color.White) // An easy optimization, don't draw unless the user specified something other than the default
						{
							var cell = new Cell
							{
								Column = visibleColumns[j],
								RowIndex = i
							};
							DrawCellBG(itemColor, cell, visibleColumns);
						}
					}
				}
			}
		}
	}
}
