/***************************************************************
 * File: Board.cs
 * Created By: Justin Grindal		Date: 27 June, 2013
 * Description: The main chess board. Board contain the chess cell
 * which will contain the chess pieces. Board also contains the methods
 * to get and set the user moves.
 ***************************************************************/

using System;
using System.Collections;
using System.Diagnostics;
using System.Xml;

namespace ChessLibrary
{
	/// <summary>
	/// he main chess board. Board contain the chess cell
	/// which will contain the chess pieces. Board also contains the methods
	/// to get and set the user moves.
	/// </summary>
    [Serializable]
	public class Board
	{
		private Side m_WhiteSide, m_BlackSide;	// Chess board site object 
		private Cells m_cells;	// collection of cells in the board

		public Board()
		{
            m_WhiteSide = new Side(Side.SideType.White);	// Makde white side
            m_BlackSide = new Side(Side.SideType.Black);	// Makde white side

			m_cells = new Cells();					// Initialize the chess cells collection
		}

		// Initialize the chess board and place piece on thier initial positions
		public void Init()
		{
			m_cells.Clear();		// Remove any existing chess cells

			// Build the 64 chess board cells
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					m_cells.Add(new Cell(row,col));	// Initialize and add the new chess cell
				}

			randomPlace();

			for (int col=1; col<=8; col++)
				m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn,m_BlackSide);

			
			for (int col=1; col<=8; col++)
				m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn,m_WhiteSide);
		}

		public void randomPlace() { 
		#region Bishop 1
		Random rng = new Random();
		int spot1 = rng.Next(0,4) * 2;
		char b1BoardCell = (char)('a' + spot1);
		m_cells[b1BoardCell+"1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
		m_cells[b1BoardCell+"8"].piece = new Piece(Piece.PieceType.Bishop,m_WhiteSide);
		#endregion

		#region Bishop 2
		int spot2 = rng.Next(0,4) * 2;
		char b2BoardCell = (char)('b' + spot2);
		m_cells[b2BoardCell + "1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
		m_cells[b2BoardCell + "8"].piece = new Piece(Piece.PieceType.Bishop,m_WhiteSide);
		#endregion
		
		#region Queen
		bool validQ = false;
		char qBoardCell;
		do { 
		int spot3 = rng.Next(0,8);
		qBoardCell = (char)('a' + spot3);
		validQ = (qBoardCell != b1BoardCell && qBoardCell != b2BoardCell);
		} while(!validQ);
		m_cells[qBoardCell + "1"].piece = new Piece(Piece.PieceType.Queen, m_BlackSide);
		m_cells[qBoardCell + "8"].piece = new Piece(Piece.PieceType.Queen,m_WhiteSide);
		#endregion

		#region Knight 1
		bool validK1 = false;
		char k1BoardCell;
		do { 
		int spot4 = rng.Next(0,8);
		k1BoardCell = (char)('a' + spot4);
		validK1 = (k1BoardCell != b1BoardCell && k1BoardCell != b2BoardCell && k1BoardCell != qBoardCell);
		} while(!validK1);
		
		m_cells[k1BoardCell + "1"].piece = new Piece(Piece.PieceType.Knight,m_BlackSide);
		m_cells[k1BoardCell + "8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
		#endregion
	
		#region Knight 2
		bool validK2 = false;
		char k2BoardCell;
		do { 
		int spot5 = rng.Next(0,8);
		k2BoardCell = (char)('a' + spot5);
		validK2 = (k2BoardCell != b1BoardCell && k2BoardCell != b2BoardCell && k2BoardCell != qBoardCell && k2BoardCell != k1BoardCell);
		} while(!validK2);
		
		m_cells[k2BoardCell + "1"].piece = new Piece(Piece.PieceType.Knight,m_BlackSide);
		m_cells[k2BoardCell + "8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
		#endregion

		#region King/Rook
		int spot6 = 0;
		for (int i = 0; i < 8; i++) {
		char BoardCell = (char)('a' + i);
		//Debug.Write(b1BoardCell + "|" + b2BoardCell + "|" + qBoardCell + "|" + k1BoardCell + "|" + k2BoardCell + "|||" + BoardCell + "\n");
		if (BoardCell != b1BoardCell && BoardCell != b2BoardCell && BoardCell != qBoardCell && BoardCell != k1BoardCell && BoardCell != k2BoardCell) { 
		switch(spot6) {
		case 0: 
		m_cells[BoardCell + "1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
		m_cells[BoardCell + "8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
		spot6++;
		break;
		case 1: 
		m_cells[BoardCell + "1"].piece = new Piece(Piece.PieceType.King, m_BlackSide);
		m_cells[BoardCell + "8"].piece = new Piece(Piece.PieceType.King, m_WhiteSide);
		spot6++;
		break;
		case 2: 
		m_cells[BoardCell + "1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
		m_cells[BoardCell + "8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
		spot6++;
		return;
		default: Debug.Write("Error: out of range"); break;
		#endregion
		}

		}

		}

		}

		// get the new item by rew and column
		public Cell this[int row, int col]
		{
			get
			{
				return m_cells[row, col];
			}
		}

		// get the new item by string location
		public Cell this[string strloc]
		{
			get
			{
				return m_cells[strloc];	
			}
		}

		// get the chess cell by given cell
		public Cell this[Cell cellobj]
		{
			get
			{
				return m_cells[cellobj.ToString()];	
			}
		}

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlBoard = xmlDoc.CreateElement("Board");

            // Append game state attributes
            xmlBoard.AppendChild(m_WhiteSide.XmlSerialize(xmlDoc));
            xmlBoard.AppendChild(m_BlackSide.XmlSerialize(xmlDoc));

            xmlBoard.AppendChild(m_cells.XmlSerialize(xmlDoc));

            // Return this as String
            return xmlBoard;
        }

        /// <summary>
        /// DeSerialize the Board object from XML String
        /// </summary>
        /// <returns>XML containing the Board object state XML</returns>
        public void XmlDeserialize(XmlNode xmlBoard)
        {
            // Deserialize the Sides XML
            XmlNode side = XMLHelper.GetFirstNodeByName(xmlBoard, "Side");

            // Deserialize the XML nodes
            m_WhiteSide.XmlDeserialize(side);
            m_BlackSide.XmlDeserialize(side.NextSibling);

            // Deserialize the Cells
            XmlNode xmlCells = XMLHelper.GetFirstNodeByName(xmlBoard, "Cells");
            m_cells.XmlDeserialize(xmlCells);
        }

		// get all the cell locations on the chess board
		public ArrayList GetAllCells()
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					CellNames.Add(this[row,col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// get all the cell containg pieces of given side
        public ArrayList GetSideCell(Side.SideType PlayerSide)
		{
			ArrayList CellNames = new ArrayList();

			// Loop all the squars and store them in Array List
			for (int row=1; row<=8; row++)
				for (int col=1; col<=8; col++)
				{
					// check and add the current type cell
					if (this[row,col].piece!=null && !this[row,col].IsEmpty() && this[row,col].piece.Side.type == PlayerSide)
						CellNames.Add(this[row,col].ToString()); // append the cell name to list
				}

			return CellNames;
		}

		// Returns the cell on the top of the given cell
		public Cell TopCell(Cell cell)
		{
			return this[cell.row-1, cell.col];
		}

		// Returns the cell on the left of the given cell
		public Cell LeftCell(Cell cell)
		{
			return this[cell.row, cell.col-1];
		}

		// Returns the cell on the right of the given cell
		public Cell RightCell(Cell cell)
		{
			return this[cell.row, cell.col+1];
		}

		// Returns the cell on the bottom of the given cell
		public Cell BottomCell(Cell cell)
		{
			return this[cell.row+1, cell.col];
		}

		// Returns the cell on the top-left of the current cell
		public Cell TopLeftCell(Cell cell)
		{
			return this[cell.row-1, cell.col-1];
		}

		// Returns the cell on the top-right of the current cell
		public Cell TopRightCell(Cell cell)
		{
			return this[cell.row-1, cell.col+1];
		}

		// Returns the cell on the bottom-left of the current cell
		public Cell BottomLeftCell(Cell cell)
		{
			return this[cell.row+1, cell.col-1];
		}

		// Returns the cell on the bottom-right of the current cell
		public Cell BottomRightCell(Cell cell)
		{
			return this[cell.row+1, cell.col+1];
		}
	}
}
