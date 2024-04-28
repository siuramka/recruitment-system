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
import BackgroundImgae from "@/assets/bg-v.jpg";

const formSchema = z.object({
  email: z.string().email().min(2, {
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
      navigate("/internships");
    } else {
      alert("Login failed");
    }
  };

  return (
    <div className="container grid h-screen w-screen flex-col items-center justify-center lg:max-w-none lg:grid-cols-2 lg:px-0">
      <div className="hidden h-full bg-muted lg:block">
        <img
          src={BackgroundImgae}
          alt="Your Image"
          className="h-[100vh] w-full"
        />
      </div>
      <div className="lg:p-8">
        <div className="mx-auto flex w-full flex-col justify-center space-y-6 sm:w-[350px]">
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
              <div className="flex justify-between">
                <Button
                  variant={"outline"}
                  onClick={() => navigate("/sign-up/user")}
                >
                  Register as User
                </Button>
                <Button
                  variant={"outline"}
                  onClick={() => navigate("/sign-up/company")}
                >
                  Register as Company
                </Button>
              </div>
            </form>
          </Form>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
