using System;

namespace updatedChessGame
{
    class ChessGameLauncher
    {
        static void Main(string[] args)
        {
            new ChessGame().play();
        }
    }

    class King : Piece
    {
        public King(bool pieceColor) : base(pieceColor) { }
        public override bool isLegalMove(Piece[,] board, Move move)
        {
            int[] difRows = new int[] { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] difColumns = new int[] { -1, 0, 1, -1, 1, -1, 0, 1 };

            if (board[move.getToRow(), move.getToColumn()] is Empty || board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor())
                for (int i = 0; i < 8; i++)
                    if (move.getFromRow() - move.getToRow() == difRows[i] && move.getFromColumn() - move.getToColumn() == difColumns[i])
                        return true;
            return false;
        }
        public override Piece copy()
        {
            King copyKing = new King(this.getPieceColor());
            copyKing.setHasMoved(this.getHasMoved());
            copyKing.setIsCaptured(this.getIsCaptured());
            return copyKing;

        }
        public override string ToString() { return this.getPieceColor() ? "WK" : "BK"; }
    }

    class Bishop : Piece
    {
        public Bishop(bool pieceColor) : base(pieceColor) { }
        public override bool isLegalMove(Piece[,] board, Move move)
        {
            int difRows = move.getFromRow() - move.getToRow();
            int difColumns = move.getFromColumn() - move.getToColumn();

            //Absolute value.
            if (difRows < 0)
                difRows = -difRows;
            if (difColumns < 0)
                difColumns = -difColumns;

            // checks if the distatnce is the same in the row and the column;
            if (difRows == difColumns)
            {
                // Possible end position.
                int numberOfMoves = difRows - 1;
                int rowPosition = move.getFromRow();
                int columnPosition = move.getFromColumn();
                int rowDirection = move.getFromRow() - move.getToRow() > 0 ? -1 : 1;
                int columnDirection = move.getFromColumn() - move.getToColumn() > 0 ? -1 : 1;
                for (int i = 0; i < numberOfMoves; i++)
                {
                    rowPosition += rowDirection;
                    columnPosition += columnDirection;
                    if (!(board[rowPosition, columnPosition] is Empty))
                        return false;
                }
                if (board[move.getToRow(), move.getToColumn()] is Empty || board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor())
                    return true;
            }
            return false;
        }
        public override Piece copy()
        {
            Bishop copyBishop = new Bishop(this.getPieceColor());
            copyBishop.setIsCaptured(this.getIsCaptured());
            copyBishop.setHasMoved(this.getHasMoved());
            return copyBishop;
        }
        public override string ToString() { return this.getPieceColor() ? "WB" : "BB"; }
    }

    class Rook : Piece
    {
        public Rook(bool pieceColor) : base(pieceColor) { }
        public override bool isLegalMove(Piece[,] board, Move move)
        {
            int difRows = move.getFromRow() - move.getToRow();
            int difColumns = move.getFromColumn() - move.getToColumn();

            //Absolute value.
            if (difRows < 0)
                difRows = -difRows;
            if (difColumns < 0)
                difColumns = -difColumns;

            if (difRows > 0 ^ difColumns > 0)
            {
                // Moving the piece up and down the column.
                if (difRows > 0)
                {
                    // Possible end position.
                    int numberOfMoves = difRows - 1;
                    int rowPosition = move.getFromRow();
                    int rowDirection = move.getFromRow() - move.getToRow() > 0 ? -1 : 1;
                    for (int i = 0; i < numberOfMoves; i++)
                    {
                        rowPosition += rowDirection;
                        if (!(board[rowPosition, move.getFromColumn()] is Empty))
                            return false;
                    }
                    if (board[move.getToRow(), move.getToColumn()] is Empty || board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor())
                        return true;
                }

                // moving the piece up and down the row.
                if (difColumns > 0)
                {
                    // Possible end position.
                    int numberOfMoves = difColumns - 1;
                    int columnPosition = move.getFromColumn();
                    int columnDirection = move.getFromColumn() - move.getToColumn() > 0 ? -1 : 1;
                    for (int i = 0; i < numberOfMoves; i++)
                    {
                        columnPosition += columnDirection;
                        if (!(board[move.getFromRow(), columnPosition] is Empty))
                            return false;
                    }
                    if (board[move.getToRow(), move.getToColumn()] is Empty || board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor())
                        return true;
                }
            }
            return false;

        }
        public override Piece copy()
        {
            Rook copyRook = new Rook(this.getPieceColor());
            copyRook.setHasMoved(this.getHasMoved());
            copyRook.setIsCaptured(this.getIsCaptured());
            return copyRook;
        }
        public override string ToString() { return this.getPieceColor() ? "WR" : "BR"; }
    }

    class Queen : Piece
    {
        Bishop queenAsBishop;
        Rook queenAsRook;
        public Queen(bool pieceColor) : base(pieceColor)
        {
            this.queenAsBishop = new Bishop(pieceColor);
            this.queenAsRook = new Rook(pieceColor);
        }
        public override bool isLegalMove(Piece[,] board, Move move)
        {
            return queenAsBishop.isLegalMove(board, move) || queenAsRook.isLegalMove(board, move);
        }
        public override Piece copy()
        {
            Queen copyQueen = new Queen(this.getPieceColor());
            copyQueen.setHasMoved(this.getHasMoved());
            copyQueen.setIsCaptured(this.getIsCaptured());
            return copyQueen;
        }
        public override string ToString() { return this.getPieceColor() ? "WQ" : "BQ"; }
    }

    class Knight : Piece
    {
        public Knight(bool pieceColor) : base(pieceColor) { }
        public override bool isLegalMove(Piece[,] board, Move move)
        {
            int[] difRows = new int[] { 2, 2, 1, 1, -1, -1, -2, -2 };
            int[] difColumns = new int[] { 1, -1, 2, -2, 2, -2, 1, -1 };
            if (board[move.getToRow(), move.getToColumn()] is Empty || board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor())
                for (int i = 0; i < 8; i++)
                    if (move.getFromRow() - move.getToRow() == difRows[i] && move.getFromColumn() - move.getToColumn() == difColumns[i])
                        return true;
            return false;
        }
        public override Piece copy()
        {
            Knight copyKnight = new Knight(this.getPieceColor());
            copyKnight.setHasMoved(this.getHasMoved());
            copyKnight.setIsCaptured(this.getIsCaptured());
            return copyKnight;
        }
        public override string ToString() { return this.getPieceColor() ? "WN" : "BN"; }
    }

    class Pawn : Piece
    {
        bool didTwoSteps;
        public Pawn(bool pieceColor) : base(pieceColor) { }
        public bool getDidTwoSteps() { return didTwoSteps; }
        public void setDidTwoSteps(bool didTwoSteps) { this.didTwoSteps = didTwoSteps; }
        public override bool isLegalMove(Piece[,] board, Move move)
        {
            int[] difRows = this.getPieceColor() ? new int[] { 1, 2, 1, 1 } : new int[] { -1, -2, -1, -1 };
            int[] difColumns = new int[] { 0, 0, -1, 1 };

            // Checks if the pawn can move 1 step.
            if (board[move.getToRow(), move.getToColumn()] is Empty && move.getFromColumn() - move.getToColumn() == difColumns[0] && move.getFromRow() - move.getToRow() == difRows[0])
                return true;

            // Checks if the pawn can 2 step.
            if (!this.getHasMoved() && board[move.getToRow(), move.getToColumn()] is Empty && move.getFromColumn() - move.getToColumn() == difColumns[1] && move.getFromRow() - move.getToRow() == difRows[1])
                return true;

            // Checks if it the pawn can capture an enemy piece diagnoly right from him.
            if (!(board[move.getToRow(), move.getToColumn()] is Empty) && board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor() && move.getFromRow() - move.getToRow() == difRows[2] && move.getFromColumn() - move.getToColumn() == difColumns[2])
                return true;

            // Checks if it the pawn can capture an enemy piece diagnoly left from him.
            if (!(board[move.getToRow(), move.getToColumn()] is Empty) && board[move.getToRow(), move.getToColumn()].getPieceColor() != this.getPieceColor() && move.getFromRow() - move.getToRow() == difRows[3] && move.getFromColumn() - move.getToColumn() == difColumns[3])
                return true;

            // Checks if the pawn can en passant capture.
            if (canEnpassantCapture(board, move))
                return true;
            return false;
        }
        public bool canEnpassantCapture(Piece[,] board, Move move)
        {
            int[] difRows = this.getPieceColor() ? new int[] { 1, 1 } : new int[] { -1, -1 };
            int[] difColumns = new int[] { -1, 1 };

            // Diagnol right.
            if (board[move.getToRow(), move.getToColumn()] is Empty && move.getFromRow() - move.getToRow() == difRows[0] && move.getFromColumn() - move.getToColumn() == difColumns[0] && board[move.getFromRow(), move.getFromColumn() + 1] is Pawn && ((Pawn)board[move.getFromRow(), move.getFromColumn() + 1]).getDidTwoSteps() && board[move.getFromRow(), move.getFromColumn() + 1].getPieceColor() != this.getPieceColor())
                return true;

            // Diagnol left.
            if (board[move.getToRow(), move.getToColumn()] is Empty && move.getFromRow() - move.getToRow() == difRows[1] && move.getFromColumn() - move.getToColumn() == difColumns[1] && board[move.getFromRow(), move.getFromColumn() - 1] is Pawn && ((Pawn)board[move.getFromRow(), move.getFromColumn() - 1]).getDidTwoSteps() && board[move.getFromRow(), move.getFromColumn() - 1].getPieceColor() != this.getPieceColor())
                return true;
            return false;
        }
        public override bool Equals(Piece piece) { return base.Equals(piece) && piece is Pawn && ((Pawn)piece).getDidTwoSteps() == this.didTwoSteps; }
        public override Piece copy()
        {
            Pawn copyPawn = new Pawn(this.getPieceColor());
            copyPawn.setDidTwoSteps(this.getDidTwoSteps());
            copyPawn.setHasMoved(this.getHasMoved());
            copyPawn.setIsCaptured(this.getIsCaptured());
            return copyPawn;
        }
        public override string ToString() { return this.getPieceColor() ? "WP" : "BP"; }
    }

    class Empty : Piece
    {
        public override bool isLegalMove(Piece[,] board, Move move) { return false; }
        public override Piece copy() { return new Empty(); }
        public override bool Equals(Piece piece) { return base.Equals(piece); }
        public override string ToString() { return "  "; }
    }
    class Piece
    {
        bool pieceColor;
        bool hasMoved;
        bool isCaptured;
        public Piece(bool pieceColor) { this.pieceColor = pieceColor; }
        public Piece() { }
        public bool getHasMoved() { return hasMoved; }
        public void setHasMoved(bool hasMoved) { this.hasMoved = hasMoved; }
        public bool getPieceColor() { return pieceColor; }
        public bool getIsCaptured() { return isCaptured; }
        public void setIsCaptured(bool isCaptured) { this.isCaptured = isCaptured; }
        public virtual bool isLegalMove(Piece[,] board, Move move) { return false; }
        public virtual Piece copy() { return this.copy(); }
        public virtual bool Equals(Piece piece) { return pieceColor == piece.getPieceColor() && hasMoved == piece.getHasMoved() && isCaptured == piece.getIsCaptured(); }
    }

    class Move
    {
        bool playerTurn;
        int fromRow;
        int fromColumn;
        int toRow;
        int toColumn;
        Piece pieceMoved;
        public Move(bool playerTurn, int fromRow, int fromColumn, int toRow, int toColumn, Piece pieceMoved)
        {
            this.playerTurn = playerTurn;
            this.fromRow = fromRow;
            this.fromColumn = fromColumn;
            this.toRow = toRow;
            this.toColumn = toColumn;
            this.pieceMoved = pieceMoved;
        }
        public int getFromRow() { return fromRow; }
        public int getFromColumn() { return fromColumn; }
        public int getToRow() { return toRow; }
        public int getToColumn() { return toColumn; }
        public Piece getPieceMoved() { return pieceMoved; }
        public bool Equals(Move move) { return pieceMoved.Equals(move.getPieceMoved()) && fromRow == move.getFromRow() && fromColumn == move.getFromColumn() && toRow == move.getToRow() && toColumn == move.getToColumn(); }
    }

    class Player
    {
        bool playerColor;
        Piece[] arsenal;
        public Player(bool playerColor)
        {
            this.playerColor = playerColor;
            this.newArsenal();
        }
        public bool getPlayerColor() { return playerColor; }
        public void newArsenal()
        {
            arsenal = playerColor ? new Piece[] { new Rook(true), new Rook(true), new Knight(true), new Knight(true), new Bishop(true), new Bishop(true), new Queen(true), new King(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), } : new Piece[] { new Rook(false), new Rook(false), new Knight(false), new Knight(false), new Bishop(false), new Bishop(false), new Queen(false), new King(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false) };
        }        
        public void updateArsenalIfPieceCaptured(Piece piece)
        {
            foreach (Piece pieces in arsenal)
            {
                if (piece is Rook && piece is Rook && !pieces.getIsCaptured())
                {
                    piece.setIsCaptured(true);
                    return;
                }
                if (piece is Knight && piece is Knight && !pieces.getIsCaptured())
                {
                    piece.setIsCaptured(true);
                    return;
                }
                if (piece is Bishop && piece is Bishop && !pieces.getIsCaptured())
                {
                    piece.setIsCaptured(true);
                    return;
                }
                if (piece is Queen && piece is Queen && !pieces.getIsCaptured())
                {
                    piece.setIsCaptured(true);
                    return;
                }
                if (piece is Pawn && piece is Pawn && !pieces.getIsCaptured())
                {
                    piece.setIsCaptured(true);
                    return;
                }
            }
        }
        public void updateArsenalIfPieceMoved(Piece piece)
        {
            foreach (Piece pieces in arsenal)
            {
                if (piece is Rook && piece is Rook && !pieces.getHasMoved())
                {
                    piece.setHasMoved(true);
                    return;
                }
                if (piece is Knight && piece is Knight && !pieces.getHasMoved())
                {
                    piece.setHasMoved(true);
                    return;
                }
                if (piece is Bishop && piece is Bishop && !pieces.getHasMoved())
                {
                    piece.setHasMoved(true);
                    return;
                }
                if (piece is Queen && piece is Queen && !pieces.getHasMoved())
                {
                    piece.setHasMoved(true);
                    return;
                }
                if (piece is Pawn && piece is Pawn && !pieces.getHasMoved())
                {
                    piece.setHasMoved(true);
                    return;
                }
            }
        }
        public void promotePawnInArsenal(Piece piece)
        {
            for (int i = 0; i < 16; i++)
                if (!arsenal[i].getIsCaptured() && arsenal[i] is Pawn)
                {
                    arsenal[i] = piece;
                    return;
                }
        }
    }

    class ChessGame
    {
        Piece[,] board;
        Player whitePlayer;
        Player blackPlayer;
        bool playerTurn;
        bool gameStatus;
        int countFiftyMoves;
        int totalMoves;
        Piece[][,] positionHistory;
        Move[] moveHistory;
        string[] gameProgress;
        bool drawByAgreement;
        bool whiteKingInCheck;
        bool blackKinginCheck;
        bool pawnMoved;
        bool pieceCaptured;
        public ChessGame()
        {
            this.newBoard();
            this.whitePlayer = new Player(true);
            this.blackPlayer = new Player(false);
            this.playerTurn = true;
            this.gameStatus = false;
            this.gameProgress = new string[0];
            this.positionHistory = new Piece[0][,];
            this.moveHistory = new Move[0];
        }

        // Function that allows players to play chess. 
        public void play()
        {
            printBoard();
            Console.WriteLine();
            do
            {
                inputPlayerMove();
                //StaleMate or if a player quit the game.
                if (gameStatus)
                    break;
                playerTurn = playerTurn ? false : true;
                Console.WriteLine("------------------------------------------------------------------------------------------------------");
                printBoard();
                Console.WriteLine();
            } while (!gameStatus);
            gameOver();
        }

        // Function for the player to input his move and implement the move to the board.
        public void inputPlayerMove()
        {
            string messageStart, messageEnd, firstInput, secondInput;
            int fromRow, fromColumn, toRow, toColumn;
            bool validMove;
            drawByAgreement = false;
            messageStart = playerTurn ? "White player, from where would you like to move a piece, or type in RESIGN to quit the game, or DRAW in order to try to draw the game by agreement, and then press Enter:" : "Black player, from where would you like to move a piece, or type in RESIGN to quit the game, or DRAW in order to try to draw the game by agreement, and then press Enter:" ;
            messageEnd = "Where would you like to place it? Press ENTER when you are done:";
            do
            {
                Move[] allLegalMoves = generateAllLegalMoves();
                if (isCheckMate(allLegalMoves))
                    return;
                if (isCheck())
                    Console.WriteLine(playerTurn ? "White king is in CHECK. \n" : "Black king is in CHECK. \n");
                if (isStaleMate(allLegalMoves))
                    return;
                firstInput = checkInput(messageStart);
                if (isInputResign(firstInput))
                    return;
                if (isInputDraw(firstInput))
                {
                    if (totalMoves > 0 && gameProgress[gameProgress.Length - 1] == "DRAW" && gameProgress[gameProgress.Length - 2] == "DRAW")
                    {
                        gameStatus = true;
                        updateGamgeProgress("DRAWBYAGREEMENT");
                        return;
                    }
                    messageStart = playerTurn == true ? "White player, from where would you like to move a piece? Press Enter when your done:" : "Black player, from where would you like to move a piece? Press Enter when your done:";
                    firstInput = checkInput(messageStart);
                }
                fromColumn = convertingLetterToNumber(firstInput[0]);
                fromRow = int.Parse(firstInput[1] + "") - 1;
                secondInput = checkInput(messageEnd);
                toColumn = convertingLetterToNumber(secondInput[0]);
                toRow = int.Parse(secondInput[1] + "") - 1;

                // Checks if the players move is valid.
                validMove = board[fromRow, fromColumn].isLegalMove(board, new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn]));

                //Adds castling to the array of all legal moves if it is legal.
                if (isCastlingLegal(new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn])))
                {
                    allLegalMoves = addToAllLegalMoves(allLegalMoves, new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn]));
                    validMove = true;
                }

                //Checks if the input is one of the legal moves that the player can make.
                if (validMove && !isInputInAllLegalMoves(allLegalMoves, fromRow, fromColumn, toRow, toColumn))
                    validMove = false;
                
                //If it is not a valid move then the computer prints a message.
                if (!validMove)
                    Console.WriteLine("Invalid move.");
            } while (!validMove);
            totalMoves++;
            if (!playerTurn)
                countFiftyMoves++;
            implementMove(new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn]));
            addToMoveHistory(new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn]));
        }
        public void implementMove(Move move)
        {
            pawnMoved = false;
            pieceCaptured = false;
            updateGameIfPieceCaptured(move);

            // Castling and en passant capture.
            if (move.getPieceMoved() is King && isCastlingLegal(move))
                castling(move);
            if (totalMoves > 2 && move.getPieceMoved() is Pawn)
                enPassantCapture(move);
            
            // Places the piece where the player wanted.
            board[move.getToRow(), move.getToColumn()] = board[move.getFromRow(), move.getFromColumn()].copy();
            updateGameIfPieceMoved(move);

            // If the piece that was moved was a pawn then we update the game that a pawn has moved and check for promotion and double step.
            pawn(move);
            board[move.getFromRow(), move.getFromColumn()] = new Empty();
            addToPositionHistory(board);
            if (isThreeFoldRepition())
                return;

            // Fifty Move Rule
            if (isFiftyMoveRule())
                return;

            // After the player got out of check than we update his king that he is out of check.
            if (playerTurn)
                whiteKingInCheck = false;
            else
                blackKinginCheck = false;

            // If somone entered a draw then gets out of the function.
            if (drawByAgreement)
                return;

            if (board[move.getToRow(), move.getToColumn()] is Pawn && didPawnTwoStep(move))
                updateGamgeProgress("PAWNTWOSTEP");
            else
                updateGamgeProgress("REGULARMOVE");
            

        }

        // Generate all legal moves for the player.
        public Move[] generateAllLegalMoves()
        {
            Move[] allLegalMoves = new Move[0];
            bool mate;
            for (int fromRow = 0; fromRow < 8; fromRow++)
                for (int fromColumn = 0; fromColumn < 8; fromColumn++)
                    for (int toRow = 0; toRow < 8; toRow++)
                        for (int toColumn = 0; toColumn < 8; toColumn++)
                            if (!(board[fromRow, fromColumn] is Empty) && board[fromRow, fromColumn].getPieceColor() == playerTurn && board[fromRow, fromColumn].isLegalMove(board, new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn])))
                            {
                                Piece destination = board[toRow, toColumn].copy();
                                board[toRow, toColumn] = board[fromRow, fromColumn].copy();
                                board[fromRow, fromColumn] = new Empty();
                                mate = isCheck();
                                board[fromRow, fromColumn] = board[toRow, toColumn].copy();
                                board[toRow, toColumn] = destination.copy();
                                if (!mate)
                                    allLegalMoves = addToAllLegalMoves(allLegalMoves, new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn]));
                            }
            return allLegalMoves;
        }
        public Move[] addToAllLegalMoves(Move[] allLegalMoves,Move move)
        {
            Move[] newAllLegalMoves = new Move[allLegalMoves.Length + 1];
            for (int i = 0; i < allLegalMoves.Length; i++)
                newAllLegalMoves[i] = allLegalMoves[i];
            newAllLegalMoves[newAllLegalMoves.Length - 1] = move;
            return newAllLegalMoves;
        }

        // Functions for checking the players input is a valid.
        public string checkInput(string message)
        {
            bool viableInput; string input;
            do
            {
                Console.WriteLine(message);
                input = Console.ReadLine().Trim().ToUpper();
                Console.WriteLine();
                viableInput = true;
                if (message == "White player, from where would you like to move a piece, or type in RESIGN to quit the game, or DRAW in order to try to draw the game by agreement, and then press Enter:" || message == "Black player, from where would you like to move a piece, or type in RESIGN to quit the game, or DRAW in order to try to draw the game by agreement, and then press Enter:")
                {
                    if (input == "DRAW")
                        return input;
                    if (input == "RESIGN")
                        return input;
                }
                if (input.Length != 2)
                    viableInput = false;
                else
                {
                    switch (input[0])
                    {
                        case 'A': case 'B': case 'C': case 'D': case 'E': case 'F': case 'G': case 'H': break;
                        default: viableInput = false; break;
                    }
                    switch (input[1])
                    {
                        case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': break;
                        default: viableInput = false; break;
                    }
                }
                if (viableInput && message != "Where would you like to place it? Press ENTER when you are done:")
                {
                    int column = convertingLetterToNumber(input[0]);
                    if (board[int.Parse(input[1] + "") - 1, column] is Empty || board[int.Parse(input[1] + "") - 1, column].getPieceColor() != playerTurn)
                        viableInput = false;
                }
                if (!viableInput)
                    Console.WriteLine("Invalid input.");
            } while (!viableInput);
            return input;

        }
        public int convertingLetterToNumber(char letter)
        {
            int column;
            switch (letter)
            {
                case 'A': case 'a': column = 0; break;
                case 'B': case 'b': column = 1; break;
                case 'C': case 'c': column = 2; break;
                case 'D': case 'd': column = 3; break;
                case 'E': case 'e': column = 4; break;
                case 'F': case 'f': column = 5; break;
                case 'G': case 'g': column = 6; break;
                default: column = 7; break;
            }
            return column;
        }
        public bool isInputInAllLegalMoves(Move [] allLegalMoves, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            for (int i = 0; i < allLegalMoves.Length; i++)
                if (new Move(playerTurn, fromRow, fromColumn, toRow, toColumn, board[fromRow, fromColumn]).Equals(allLegalMoves[i]))
                    return true;
            return false;
        }
        
        // Functions that check if the player entered resign or draw.
        public bool isInputDraw(string firstInput)
        {
            if (firstInput == "DRAW")
            {
                updateGamgeProgress("DRAW");
                drawByAgreement = true;
                return true;
            }
            return false;
        }
        public bool isInputResign(string firstInput)
        {
            if (firstInput == "RESIGN")
            {
                gameStatus = true;
                updateGamgeProgress("RESIGN");
                return true;
            }
            return false;
        }

        // Functions for checkmate, check and stalemate.
        public bool isCheckMate(Move[] allLegalMoves)
        {
            if (isCheck() && allLegalMoves.Length == 0)
            {
                gameStatus = true;
                updateGamgeProgress("CHECKMATE");
                return true;
            }
            return false;
        }
        public bool isCheck()
        {
            string locationOfKing = findLocationOfKing();
            int rowKing = int.Parse(locationOfKing[0] + "");
            int columnKing = int.Parse(locationOfKing[1] + "");
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    if (board[row, column].getPieceColor() != playerTurn && board[row, column].isLegalMove(board, new Move(!playerTurn, row, column, rowKing, columnKing, board[row, column])))
                    {
                        if (playerTurn)
                            whiteKingInCheck = true;
                        else
                            blackKinginCheck = true;
                        return true;
                    }
            return false;           
        }
        public bool isStaleMate(Move[] allLegalMoves)
        {
            if (!isCheck() && allLegalMoves.Length == 0)
            {
                gameStatus = true;
                updateGamgeProgress("STALEMATE");
                return true;
            }
            return false;
        }

        // Find the location of the king for the player.
        public string findLocationOfKing()
        {
            string locationOfKing = "";
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    if (board[row, column] is King && board[row, column].getPieceColor() == playerTurn)
                        locationOfKing = string.Format("{0}{1}", row, column);
            return locationOfKing;
        }

        // Function to check for ThreeFold repition.
        public bool isThreeFoldRepition()
        {
            for (int i = 0; i < positionHistory.Length; i++)
            {
                int count = 0;
                Piece[,] currentPosition = positionHistory[i];
                for (int j = 0; j < positionHistory.Length; j++)
                {
                    if (Equals(currentPosition, positionHistory[j]))
                        count++;
                    if (count == 3)
                    {
                        gameStatus = true;
                        updateGamgeProgress("THREEFOLDREPITION");
                        return true;
                    } 
                }
            }
            return false;
        }

        // Funtion to check for fifty move rule and if pawn moved or a piece captured then it resets the count to 0.
        public bool isFiftyMoveRule()
        {
            if (pawnMoved || pieceCaptured)
                countFiftyMoves = 0;
            if (countFiftyMoves == 50)
            {
                gameStatus = true;
                updateGamgeProgress("FIFTYMOVERULE");
                return true;
            }
            return false;
        }

        // Prints the reason why the game is over.
        public void gameOver()
        {
            for (int i = 0; i < gameProgress.Length; i ++)
            {
                if (gameProgress[i] == "RESIGN") { Console.WriteLine(playerTurn ? "Black player WON. White player resigned." : "White player WON.Black player resigned."); return; }
                if (gameProgress[i] == "DRAWBYAGREEMENT") { Console.WriteLine("DRAW BY AGREEMENT"); return; }
                if (gameProgress[i] == "FIFTYMOVERULE") { Console.WriteLine("FIFTY MOVE RULE"); return; }
                if (gameProgress[i] == "THREEFOLDREPITION") { Console.WriteLine("THREEFOLD REPITION"); return; }
                if (gameProgress[i] == "STALEMATE") { Console.WriteLine("STALEMATE"); return; }
                if (gameProgress[i] == "CHECKMATE") { Console.WriteLine(playerTurn ? "CHECKMATE. Black player won." : "CHECKMATE. White player won."); return; }
            }
        }

        // Functions for castling.
        public bool isCastlingLegal(Move move)
        {
            if (playerTurn ? !whiteKingInCheck : !blackKinginCheck)
            {
                int toRow = playerTurn ? 7 : 0;
                int[] toColumn = { 6, 2 };

                if (move.getToRow() == toRow && move.getToColumn() == toColumn[0])
                {
                    if (board[toRow, 4] is King && board[toRow, 4].getHasMoved() || board[toRow, 7] is Rook && board[toRow, 7].getHasMoved())
                        return false;
                    for (int i = 5; i < 7; i++)
                        if (!(board[toRow, i] is Empty))
                            return false;
                    return true;
                }
                if (move.getToRow() == toRow && move.getToColumn() == toColumn[1])
                {
                    if (board[toRow, 4] is King && board[toRow, 4].getHasMoved() || board[toRow, 7] is Rook && board[toRow, 0].getHasMoved())
                        return false;
                    for (int i = 1; i < 4; i++)
                        if (!(board[toRow, i] is Empty))
                            return false;
                    return true;
                }
            }
            return false;
        }
        public void castling(Move move)
        {
            int toRow = playerTurn ? 7 : 0;
            int[] toColumn = { 6, 2 };
            if (move.getToColumn() == 6)
            {
                board[move.getToRow(), 5] = board[move.getToRow(), 7].copy();
                board[move.getToRow(), 7] = new Empty();
            }    
            else
            {
                board[move.getToRow(), 3] = board[move.getToRow(), 0].copy();
                board[move.getToRow(), 0] = new Empty();
            }
        }

        // Pawn promotion and en passant capture.
        public void promotion(Move move)
        {
            Player player = playerTurn ? whitePlayer : blackPlayer;
            string input;
            bool viableInput;
            if (move.getToRow() == (playerTurn ? 0 : 7))
            {
                do
                {
                    viableInput = true;
                    Console.WriteLine(playerTurn ? "White player enter Q: Queen, R: Rook, B: Bishop, K: Knight" : "Black player enter Q: Queen, R: Rook, B: Bishop, K: Knight");
                    input = Console.ReadLine().Trim().ToUpper();
                    if (input.Length != 1)
                        viableInput = false;
                    switch (input[0])
                    {
                        case 'Q': case 'R': case 'B': case 'K': break;
                        default: viableInput = false; break;
                    }
                } while (!viableInput);
                switch (input[0])
                {
                    case 'Q': board[move.getToRow(), move.getToColumn()] = new Queen(playerTurn ? true : false); player.promotePawnInArsenal(new Queen(playerTurn ? true : false)); break;
                    case 'R': board[move.getToRow(), move.getToColumn()] = new Rook(playerTurn ? true : false); player.promotePawnInArsenal(new Rook(playerTurn ? true : false)); break;
                    case 'B': board[move.getToRow(), move.getToColumn()] = new Bishop(playerTurn ? true : false); player.promotePawnInArsenal(new Bishop(playerTurn ? true : false)); break;
                    case 'K': board[move.getToRow(), move.getToColumn()] = new Knight(playerTurn ? true : false); player.promotePawnInArsenal(new Knight(playerTurn ? true : false)); break;
                }
            }
        }
        public void enPassantCapture(Move move)
        {
            int[] difRows = playerTurn ? new int[] { 1, 1 } : new int[] { -1, -1 };
            int[] difColumns = new int[] { -1, 1 };

            // Diagnol right.
            if (move.getFromRow() - move.getToRow() == difRows[0] && move.getFromColumn() - move.getToColumn() == difColumns[0])
            {
                (playerTurn ? blackPlayer : whitePlayer).updateArsenalIfPieceCaptured(board[move.getFromRow(), move.getFromColumn() + 1]);
                board[move.getFromRow(), move.getFromColumn() + 1] = new Empty(); 
            }
            // Diagnol left.
            if (move.getFromRow() - move.getToRow() == difRows[1] && move.getFromColumn() - move.getToColumn() == difColumns[1])
            {
                (playerTurn ? blackPlayer : whitePlayer).updateArsenalIfPieceCaptured(board[move.getFromRow(), move.getFromColumn() - 1]);
                board[move.getFromRow(), move.getFromColumn() - 1] = new Empty();
            }

        }

        // Add to position history.
        public void addToPositionHistory(Piece[,] position)
        {
            Piece[][,] newPositionHistory = new Piece[positionHistory.Length + 1][,];
            for (int i = 0; i < positionHistory.Length; i++)
                newPositionHistory[i] = copy(positionHistory[i]);
            newPositionHistory[newPositionHistory.Length - 1] = copy(position);
            this.positionHistory = newPositionHistory;
        }

        // Add to move history.
        public void addToMoveHistory(Move move)
        {
            Move[] newMoveHistory = new Move[moveHistory.Length + 1];
            for (int i = 0; i < moveHistory.Length; i++)
                newMoveHistory[i] = moveHistory[i];
            newMoveHistory[newMoveHistory.Length - 1] = move;
            moveHistory = newMoveHistory;
        }

        // Funtion that update the game if a pawn has moved and checks if the player is able to promote, update the pawn has two steped.
        public void pawn(Move move)
        {
            if (move.getPieceMoved() is Pawn)
            {
                pawnMoved = true;
                promotion(move);           
                if (didPawnTwoStep(move))
                    ((Pawn)board[move.getToRow(), move.getToColumn()]).setDidTwoSteps(true);
            }
        }
        public bool didPawnTwoStep(Move move)
        {
            int difRows = move.getFromRow() - move.getToRow();
            if (difRows < 0)
                difRows = -difRows;
            if (move.getFromColumn() == move.getToColumn() && difRows == 2)
                return true;
            return false;
        }

        public void updateGamgeProgress(string progress)
        {
            string[] newGameProgress = new string[gameProgress.Length + 1];
            for (int i = 0; i < gameProgress.Length; i++)
                newGameProgress[i] = gameProgress[i];
            newGameProgress[newGameProgress.Length - 1] = progress;
            gameProgress = newGameProgress;
        }

        //Functions that updates the the game if a piece has been captured or moved.
        public void updateGameIfPieceCaptured(Move move)
        {
            if (!(board[move.getToRow(), move.getToColumn()] is Empty))
            {
                (playerTurn ? whitePlayer : blackPlayer).updateArsenalIfPieceCaptured(board[move.getToRow(), move.getToColumn()]);
                pieceCaptured = true;
            }
        }
        public void updateGameIfPieceMoved(Move move)
        {
            if (!move.getPieceMoved().getHasMoved())
            {
                (playerTurn ? whitePlayer : blackPlayer).updateArsenalIfPieceMoved(board[move.getToRow(), move.getToColumn()]);
                board[move.getToRow(), move.getToColumn()].setHasMoved(true);
            }
        }

        // Functions for the board.
        public void newBoard()
        {
            this.board = new Piece[,]
            {
                { new Rook(false), new Knight(false), new Bishop(false), new Queen(false), new King(false), new Bishop(false), new Knight(false), new Rook(false) },
                { new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false), new Pawn(false) },
                { new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty()},
                { new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty()},
                { new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty()},
                { new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty(), new Empty()},
                { new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true), new Pawn(true) },
                { new Rook(true), new Knight(true), new Bishop(true), new Queen(true), new King(true), new Bishop(true), new Knight(true), new Rook(true) }
            }; 
        }
        public void printBoard()
        {
            Console.WriteLine("     A    B    C    D    E    F    G    H");
            Console.WriteLine("  +----+----+----+----+----+----+----+----+");
            for (int row = 0; row < board.GetLength(0); row++)
            {
                Console.Write("{0} |", row + 1);
                for (int column = 0; column < board.GetLength(1); column++)
                    Console.Write(" {0} |", board[row, column] == null ? "  " : board[row, column].ToString());
                Console.Write(" {0} \n", row + 1);
                Console.WriteLine("  +----+----+----+----+----+----+----+----+");
            }
            Console.WriteLine("     A    B    C    D    E    F    G    H");
        }
        public Piece[,] copy(Piece[,] position)
        {
            Piece[,] copyBoard = new Piece[8, 8];
            for (int row = 0; row < 8; row++)
                for (int column = 0; column < 8; column++)
                    copyBoard[row, column] = position[row, column].copy();
            return copyBoard;
        }
        public bool Equals(Piece[,] position1, Piece[,] position2)
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (!position1[i, j].Equals(position2[i, j]))
                        return false;
            return true;
        }
    }
}
