SELECT EventName, EventSportTypeID,
    COUNT(*) AS CNT
FROM MatchSystemIDs
GROUP BY EventName,EventSportTypeID
HAVING COUNT(*) > 1;

delete  FROM MatchSystemIDs
    WHERE ID NOT IN
    (
        SELECT MAX(ID) AS MaxRecordID
        FROM MatchSystemIDs
        GROUP BY EventName, EventSportTypeID
    );


select*
from
MatchSystemIDs
where
EventSystemID = '00000000-0000-0000-0000-000000000000'

select*
from
MatchSystemIDs
where
EventName like '%Pobeda%'

select*
from
MatchSystemIDs
where
EventSportTypeID = 0