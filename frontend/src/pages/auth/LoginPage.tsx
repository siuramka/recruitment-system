"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { login } from "@/services/AuthService";
import {
  getUserFromTokens,
  saveTokensToLocalStorage,
} from "@/features/AuthSliceActions";
import { useNavigate } from "react-router-dom";
import { useDispatch } from "react-redux";
import { saveUser } from "@/features/AuthSlice";

const formSchema = z.object({
  email: z.string().min(2, {
    message: "Email must be at least 2 characters.",
  }),
  password: z.string().min(2, {
    message: "Password must be at least 2 characters.",
  }),
});

const LoginPage = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (values: z.infer<typeof formSchema>) => {
    const tokens = await login({ ...values });
    if (tokens) {
      saveTokensToLocalStorage(tokens.accessToken, tokens.refreshToken);
      dispatch(
        saveUser({
          user: getUserFromTokens(tokens.accessToken, tokens.refreshToken),
        })
      );
      navigate("/dashboard");
    } else {
      alert("Login failed");
    }
  };

  return (
    <div className="max-w-sm border border-solid w-1/2 rounded mx-auto p-5">
      <h1 className="text-3xl font-semibold mb-4 text-left">Login</h1>
      <p className="text-sm font-light mb-4 text-left pb-8">
        Enter your email and password information!
      </p>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
          <FormField
            control={form.control}
            name="email"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="flex items-start">Email</FormLabel>
                <FormControl>
                  <Input placeholder="saram@hotmail.com" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="password"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="flex items-start">Password</FormLabel>
                <FormControl>
                  <Input placeholder="" type="password" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <Button className="w-full" type="submit">
            Login
          </Button>
        </form>
      </Form>
    </div>
  );
};

export default LoginPage;
