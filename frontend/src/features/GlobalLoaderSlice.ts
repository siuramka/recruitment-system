import { RootState } from "@/app/store";
import { createSlice } from "@reduxjs/toolkit";

interface GlobalLoaderState {
  isLoading: boolean;
}

const initialState: GlobalLoaderState = {
  isLoading: false,
};

const globalLoaderSlice = createSlice({
  name: "globalLoader",
  initialState,
  reducers: {
    showLoader: (state) => {
      state.isLoading = true;
    },
    hideLoader: (state) => {
      state.isLoading = false;
    },
  },
});

export const { showLoader, hideLoader } = globalLoaderSlice.actions;
export const selectIsLoading = (state: RootState) =>
  state.globalLoader.isLoading;

export default globalLoaderSlice.reducer;
