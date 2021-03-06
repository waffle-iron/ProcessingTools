/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 e.[Name]
	  ,m.[Name]
	  ,s.[Name]
  FROM [MediaTypes].[dbo].[MimeTypePairFileExtensions] x
  JOIN [MediaTypes].[dbo].[FileExtensions] e
  ON x.[FileExtension_Id] = e.[Id]
  JOIN [MediaTypes].[dbo].[MimeTypePairs] p
  ON x.[MimeTypePair_Id] = p.[Id]
  JOIN [MediaTypes].[dbo].[MimeTypes] m
  ON p.[MimeTypeId] = m.[Id]
  JOIN [MediaTypes].[dbo].[MimeSubtypes] s
  ON p.[MimeSubtypeId] = s.[Id]
  WHERE e.[Name] = 'mp2'