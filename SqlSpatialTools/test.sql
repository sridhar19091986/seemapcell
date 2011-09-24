SELECT neatline, 'Background' as name, 'map_neatlines' as layer, 'White' as FillColor, 'Transparent' as LineColor, 1 as LineThickness FROM map_neatlines UNION ALL
SELECT shores, name, 'ponds' as layer, 'LightBlue' as FillColor, 'Blue' as LineColor, 1 as LineThickness FROM ponds UNION ALL
SELECT boundary, name, 'forests' as layer, 'Green' as FillColor, '#ff005500' as LineColor, 4 as LineThickness FROM Forests UNION ALL
SELECT shore, name, 'lakes' as layer, 'Blue' as FillColor, 'Transparent' as LineColor, 2 as LineThickness FROM lakes UNION ALL
SELECT boundary, name, 'named_places' as layer, '#00ffff99' as FillColor, 'Brown' as LineColor, 2 as LineThickness FROM named_places UNION ALL
SELECT centerline, name, 'streams_outline' as layer, 'Blue' as FillColor, 'Blue' as LineColor, 5 as LineThickness FROM streams UNION ALL
SELECT centerline, name, 'streams_fill' as layer, 'LightBlue' as FillColor, 'LightBlue' as LineColor, 3 as LineThickness FROM streams UNION ALL
SELECT centerline, name, 'road_segments' as layer, 'LightGreen' as FillColor, 'Black' as LineColor, num_lanes*2 as LineThickness FROM road_segments UNION ALL
SELECT centerlines, name, 'divided_routes' as layer, 'LightGreen' as FillColor, '#aaff0000' as LineColor, num_lanes*2 as LineThickness FROM divided_routes UNION ALL
SELECT footprint, address, 'buildings_footprint' as layer, 'Black' as FillColor, 'Transparent' as LineColor, 1 as LineThickness FROM buildings UNION ALL
SELECT position.STBuffer(0.5), address, 'buildings_position' as layer, 'Red' as FillColor, 'Transparent' as LineColor, 1 as LineThickness FROM buildings UNION ALL
SELECT position.STBuffer(1), name, 'bridges' as layer, '#550000ff' as FillColor, 'Black' as LineColor, 1 as LineThickness FROM bridges 

