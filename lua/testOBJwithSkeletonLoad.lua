
require("config")
require("module")
require("common")

package.path=package.path..";../Samples/sample_luatorch/lua/?.lua" --;"..package.path
package.path=package.path..";../../taesooLib/Samples/scripts/RigidBodyWin/subRoutines/?.lua" --;"..package.path

function ctor()

	local bgnode=RE.ogreSceneManager():getSceneNode("BackgroundNode")
	bgnode:setVisible(false)

	this:create("Button", 'showBones', 'showBones')
	this:create("Button", 'hideBones', 'hideBones')
	this:updateLayout()

	local s=8
	local h=1.3


	local mesh
	mesh=Geometry()
	mesh:loadOBJ('/home/calab/Unity_FBX_Parser/Results/Hamster.obj')
	

	-- local m=matrix4()
	-- m:setTranslation(vector3(-s*50,-0.1,-s*50), false)
	-- mesh:transform(m)
	
	--mesh:drawMesh('lightgrey_transparent')

	-- scale 100 for rendering 
	local meshToEntity=MeshToEntity(mesh, 'meshName')
	--meshToEntity:updatePositionsAndNormals()
	local entity=meshToEntity:createEntity('entityName' )

	local materialName='lightgrey_transparent'
	-- local materialName='lightgrey'
	entity:setMaterialName(materialName)
	local node=RE.createChildSceneNode(RE.ogreRootSceneNode(), "mesh_node")
	node:setScale(1000, 1000, 1000)
	node:attachObject(entity)
	--node:translate(-s*50,-0.1,-s*50)

end

function frameMove()
	return 0
end
function handleRendererEvent(ev, button, x,y)
	return 0
end
function dtor()
end
function onFrameChanged(currFrame)
end
function onCallback(w, usrData)
	if w:id()=='showBones' then
		local f = io.open('/home/calab/Unity_FBX_Parser/Results/Hamster_Skeleton.txt', "r")
		local positions = {}
		for line in f:lines("l") do
			local l = {}
			for i in string.gmatch(line, "[^%s]+") do
				print(i)
				table.insert(l, i)
			end
			table.insert(positions, vector3(tonumber(l[1])*1000, tonumber(l[2])*1000, tonumber(l[3])*1000))
		end
		for key, value in pairs(positions) do
			dbg.draw("Sphere", value, tostring(key), "blue", 1)
		end
	elseif w:id()=='hideBones' then
		dbg.eraseAllDrawn()
	end
end
