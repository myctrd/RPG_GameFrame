#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class LuaCallCSUtilsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LuaCallCSUtils);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 23, 1, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "PrintTest", _m_PrintTest_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetRoleAvailable", _m_GetRoleAvailable_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRolePlayer", _m_SetRolePlayer_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPlayerData", _m_GetPlayerData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetPlayerGold", _m_GetPlayerGold_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetPlayerEquip", _m_SetPlayerEquip_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "StartBattle", _m_StartBattle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "EndBattle", _m_EndBattle_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBattlePlayerData", _m_GetBattlePlayerData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetEnemyData", _m_GetEnemyData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RoleAttackRole", _m_RoleAttackRole_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadGameScene", _m_LoadGameScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UnloadGameScene", _m_UnloadGameScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadItemData", _m_LoadItemData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadEventData", _m_LoadEventData_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddEquip", _m_AddEquip_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "UpdateItem", _m_UpdateItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetInteraction", _m_SetInteraction_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "PlayerPrefsHasKey", _m_PlayerPrefsHasKey_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ActivateEvent", _m_ActivateEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddGold", _m_AddGold_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowItem", _m_ShowItem_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "m_instance", _g_get_m_instance);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "m_instance", _s_set_m_instance);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					LuaCallCSUtils gen_ret = new LuaCallCSUtils();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LuaCallCSUtils constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PrintTest_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSUtils.PrintTest(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRoleAvailable_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        int[] gen_ret = LuaCallCSUtils.GetRoleAvailable(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRolePlayer_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _role = LuaAPI.xlua_tointeger(L, 1);
                    
                    LuaCallCSUtils.SetRolePlayer( _role );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPlayerData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        RoleBase gen_ret = LuaCallCSUtils.GetPlayerData(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPlayerGold_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        int gen_ret = LuaCallCSUtils.GetPlayerGold(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetPlayerEquip_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _slot = LuaAPI.xlua_tointeger(L, 1);
                    int _id = LuaAPI.xlua_tointeger(L, 2);
                    
                    LuaCallCSUtils.SetPlayerEquip( _slot, _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StartBattle_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _id = LuaAPI.xlua_tointeger(L, 1);
                    
                    LuaCallCSUtils.StartBattle( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EndBattle_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSUtils.EndBattle(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBattlePlayerData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        RolePlayer gen_ret = LuaCallCSUtils.GetBattlePlayerData(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEnemyData_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        RoleEnemy gen_ret = LuaCallCSUtils.GetEnemyData(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RoleAttackRole_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    RoleBase _a = (RoleBase)translator.GetObject(L, 1, typeof(RoleBase));
                    RoleBase _b = (RoleBase)translator.GetObject(L, 2, typeof(RoleBase));
                    
                    LuaCallCSUtils.RoleAttackRole( _a, _b );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadGameScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSUtils.LoadGameScene(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadGameScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSUtils.UnloadGameScene(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadItemData_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSUtils.LoadItemData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadEventData_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LuaCallCSUtils.LoadEventData(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEquip_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _id = LuaAPI.lua_tostring(L, 1);
                    string _count = LuaAPI.lua_tostring(L, 2);
                    
                    LuaCallCSUtils.AddEquip( _id, _count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UpdateItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _id = LuaAPI.lua_tostring(L, 1);
                    string _count = LuaAPI.lua_tostring(L, 2);
                    
                    LuaCallCSUtils.UpdateItem( _id, _count );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetInteraction_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    bool _state = LuaAPI.lua_toboolean(L, 1);
                    
                    LuaCallCSUtils.SetInteraction( _state );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayerPrefsHasKey_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    
                        bool gen_ret = LuaCallCSUtils.PlayerPrefsHasKey( _key );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ActivateEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _eventID = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSUtils.ActivateEvent( _eventID );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddGold_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _value = LuaAPI.xlua_tointeger(L, 1);
                    
                    LuaCallCSUtils.AddGold( _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _id = LuaAPI.lua_tostring(L, 1);
                    
                    LuaCallCSUtils.ShowItem( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_m_instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, LuaCallCSUtils.m_instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_m_instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    LuaCallCSUtils.m_instance = (LuaCallCSUtils)translator.GetObject(L, 1, typeof(LuaCallCSUtils));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
