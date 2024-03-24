import { configureStore } from "@reduxjs/toolkit";
import authReducer from "../features/AuthSlice";
import globalLoaderReducer from "../features/GlobalLoaderSlice";
// ...

export const store = configureStore({
  reducer: {
    auth: authReducer,
    globalLoader: globalLoaderReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
