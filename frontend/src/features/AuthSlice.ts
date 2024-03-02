import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  getUserFromLocalStorage,
  removeUserFromLocalStorage,
} from "./AuthSliceActions";
import { RootState } from "../app/store";

export type User = {
  id: string;
  role: string;
  email: string;
  exp: number;
};

export type AuthState = {
  user: User | null;
};

const initialState: AuthState = {
  user: getUserFromLocalStorage(),
};

type SetUserPayload = {
  user: User;
};

const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    saveUser: (state, action: PayloadAction<SetUserPayload>) => {
      state.user = action.payload.user;
    },
    removeUser: (state) => {
      state.user = null;
      removeUserFromLocalStorage();
    },
  },
});

export const selectUser = (state: RootState) => state.auth.user;

export const { saveUser, removeUser } = authSlice.actions;

export default authSlice.reducer;
