select *
from OddsTable s
join (
    select HomeSystemID, AwaySystemID, SportTypeID,  count(*) as qty
    from OddsTable
	where HomeSystemID != '00000000-0000-0000-0000-000000000000' and AwaySystemID != '00000000-0000-0000-0000-000000000000' 
    group by HomeSystemID, AwaySystemID,SportTypeID
    having count(*) > 1
)
 t on s.HomeSystemID = t.HomeSystemID and s.AwaySystemID = t.AwaySystemID
 where s.HomeSystemID != '00000000-0000-0000-0000-000000000000' and s.AwaySystemID != '00000000-0000-0000-0000-000000000000' 
 order by t.HomeSystemID, t.AwaySystemID

