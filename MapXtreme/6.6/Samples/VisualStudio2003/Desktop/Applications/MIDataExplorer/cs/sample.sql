#you can use the path command to point to a location to find tables.
#path \data\testscripts\english\

# if you are going to modify data, you might want to make a copy of the usa table, and modify that
# open copyofusa.tab as usa

open usa.tab
open mexico.tab

#
# Define some parameters
#
SET @MILLION=1000000
SET @NewYork='NY'
SET @SoundsLike='N%'
#
# Test some projection lists and projection list aliases
#
select state from usa
select state, state_name from usa
select state AS foo from usa
select state AS foo, state_name AS bar from usa
select state foo from usa
select state foo, state_name AS bar from usa
select Pop_90 as foo, Pop_90 / 100 as bar from mexico
select Pop_90 as foo, Pop_90 / 100 as bar from mexico where bar > 10000000
select A.Pop_90 as foo, A.Pop_90 / 100 as bar from mexico As A where bar < A.Pop_90
#
# Test table aliases
#
select A.state from usa AS A
select A.state from usa A
select A.state As foo from usa AS A
#
# Test the "*" projection list
#
select * from usa
select usa.* from usa
select A.* from usa as a
#
# Test some row expressions
#
select state_name, Households_90 / Cars_91 from mexico where Households_90 / Cars_91 < 1.0
select state_name, Households_90 / Cars_91 As foo from mexico where foo < 1.0
select state, state_name from usa where state like @SoundsLike
select state, state_name from usa where state like @SoundsLike And state <> @NewYork And Pop_90 > @MILLION
#
# Test order By
#
select state_name, Pop_90 from mexico order by Pop_90
select state_name, Pop_90 from mexico order by 2
select state_name, Pop_90 As foo from mexico order by foo
select state_name, Pop_90 / Households_90 As Bar, Pop_90 As foo from mexico order by foo
select state_name, Pop_90 from mexico order by Pop_90 asc
select state_name, Pop_90 from mexico order by Pop_90 desc
#
# Do some selecting based on the MI_Key pseudo column
#
select MI_Key, state_name, Pop_90 from mexico
select state, state_name from usa where MI_Key = '5'
#
# Perform a spatial query - Let's find all the usa that are within 100 miles of Wyoming.
# First we need to get a FeatureGeometry that represents a 100 mile buffer around Wyoming.
# Then we will use that in the where-clause of a select statement.
#
SET @WyomingBuffer=select MI_Buffer(Obj, 100, 'mi', 'Spherical', 4) from usa where state = 'WY'
select state, state_name from usa where Obj intersects @WyomingBuffer
#
# Close the tables
#
close all

##########################################################################
# These statements won't execute because of the above close all command.
# They are here to illustrate some insert, update, and delete commands
##########################################################################
SET @NewState='NS'
insert into mexico (state_code, state_name, Pop_90) values (@NewState, 'New State Name', @MILLION)
update mexico set Pop_90 = Pop_90 * 2 where state_code = @NewState
delete from mexico where state_code = @NewState
insert into mexico (state_code, state_name, Pop_90) select state_code, state_name, Pop_90 from mexico
insert into mexico (Obj, state_code, state_name, Pop_90) select Obj, @NewState, 'New State Name', @MILLION from mexico where state_code = '01'


