CREATE PROCEDURE [dbo].[SimpleInsertSproc]
	@param1 nvarchar(255),
	@param2 int
AS
	INSERT INTO SimpleTable (SimpleText,SimpleInt) VALUES (@param1, @param2)
RETURN 0
